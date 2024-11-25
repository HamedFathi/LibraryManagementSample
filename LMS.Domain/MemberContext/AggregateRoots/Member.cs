﻿using HamedStack.TheAggregateRoot;
using HamedStack.TheAggregateRoot.Abstractions;
using HamedStack.TheResult;
using LMS.Domain.BookContext.AggregateRoots;
using LMS.Domain.BookContext.Entities;
using LMS.Domain.BookContext.Enumerations;
using LMS.Domain.FineContext.Entities;
using LMS.Domain.FineContext.Enumerations;
using LMS.Domain.LoanContext.Entities;
using LMS.Domain.LoanContext.ValueObjects;
using LMS.Domain.MemberContext.Enumerations;
using LMS.Domain.MemberContext.ValueObjects;
using LMS.Domain.PaymentContext.Entities;
using LMS.Domain.PaymentContext.Enumerations;
using LMS.Domain.ReservationContext.Entities;
using LMS.Domain.ReservationContext.Enumerations;
using LMS.Domain.SharedKernel.Enumerations;
using LMS.Domain.SharedKernel.ValueObjects;
using LMS.Domain.UserContext.Entities;

namespace LMS.Domain.MemberContext.AggregateRoots;

public class Member : Entity<Guid>, IAggregateRoot
{
    public FullName FullName { get; }
    public MembershipType MembershipType { get; }
    public ContactInfo ContactInfo { get; }
    public MembershipDuration Duration { get; }
    public Address Address { get; }
    public Currency PreferredCurrency { get; }

    public User? User { get; private set; }

    private readonly List<Loan> _loans = new();
    private readonly List<Reservation> _reservations = new();
    private readonly List<Fine> _fines = new();
    private readonly List<Payment> _payments = new();

    public IReadOnlyList<Loan> Loans => _loans.AsReadOnly();
    public IReadOnlyList<Reservation> Reservations => _reservations.AsReadOnly();
    public IReadOnlyList<Fine> Fines => _fines.AsReadOnly();
    public IReadOnlyList<Payment> Payments => _payments.AsReadOnly();

    private Member(FullName fullName, MembershipType membershipType, ContactInfo contactInfo, MembershipDuration duration, Address address, Currency preferredCurrency)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        MembershipType = membershipType;
        ContactInfo = contactInfo;
        Duration = duration;
        Address = address;
        PreferredCurrency = preferredCurrency;
    }

    public static Result<Member> Create(string firstName, string lastName, MembershipType membershipType, ContactInfo contactInfo, MembershipDuration duration, Address address, Currency preferredCurrency)
    {
        var nameResult = FullName.Create(firstName, lastName);
        if (!nameResult.IsSuccess)
            return Result<Member>.Failure(nameResult.Errors);

        return Result<Member>.Success(new Member(nameResult.Value!, membershipType, contactInfo, duration, address, preferredCurrency));
    }
    public Result<bool> LinkUser(User user)
    {
        if (User != null)
            return Result<bool>.Failure("This member already has a linked account.");

        User = user;
        return Result<bool>.Success(true);
    }

    public Result<Fine> IssueFine(Loan loan, Money amount)
    {
        var fineResult = Fine.Create(loan, amount);
        if (!fineResult.IsSuccess)
            return fineResult;

        _fines.Add(fineResult.Value!);
        return fineResult;
    }

    public Result<bool> CanRenewMembership()
    {
        if (_fines.Any(f => f.Status == FineStatus.Unpaid))
            return Result<bool>.Failure("You cannot renew your membership until all fines are paid.");

        return Result<bool>.Success(true);
    }

    public Result<bool> CanBorrow()
    {
        if (_fines.Any(fine => fine.Status == FineStatus.Unpaid))
            return Result<bool>.Failure("Member has unpaid fines.");

        if (_loans.Count >= MembershipType.MaxLoans)
            return Result<bool>.Failure("Loan limit reached.");

        if (Duration.IsExpired())
            return Result<bool>.Failure("Membership has expired.");

        return Result<bool>.Success(true);
    }

    public Result<Loan> BorrowBook(BookCopy copy, LoanPeriod loanPeriod)
    {
        var canBorrowResult = CanBorrow();
        if (!canBorrowResult.IsSuccess)
            return Result<Loan>.Failure(canBorrowResult.Errors);

        if (copy.CurrentStatus != BookCopyStatus.Available)
            return Result<Loan>.Failure("Book copy is not available for loan.");

        if (copy.Condition == CopyCondition.Damaged)
            return Result<Loan>.Failure("This book is in poor condition and cannot be loaned.");

        var loan = Loan.Create(this, copy, loanPeriod);
        if (!loan.IsSuccess) return loan;
        _loans.Add(loan.Value!);
        copy.CheckOut();
        return loan;
    }

    public Result<bool> ReturnBook(Loan loan)
    {
        var canReturnResult = loan.CanReturn();
        if (!canReturnResult.IsSuccess)
            return Result<bool>.Failure(canReturnResult.Errors);

        loan.Return();
        if (loan.IsOverdue())
        {
            loan.MarkAsOverdue();
            var overdueDays = (DateTime.Now - loan.LoanPeriod.EndDate).Days;
            var overdueFineAmount = Money.Create(overdueDays * 1, PreferredCurrency);
            if (overdueFineAmount.IsSuccess)
                IssueFine(loan, overdueFineAmount!);
        }
        return Result<bool>.Success(true);
    }

    public Result<bool> CanReserve(Book book)
    {
        if (_reservations.Count >= MembershipType.MaxReservations)
            return Result<bool>.Failure("Reservation limit reached.");

        if (_reservations.Any(r => r.Book.Id == book.Id && r.Status == ReservationStatus.Pending))
            return Result<bool>.Failure("Book is already reserved by this member.");

        if (!book.GetAvailableCopies().Any())
            return Result<bool>.Failure("No copies available for reservation.");

        return Result<bool>.Success(true);
    }

    public Result<Reservation> PlaceReservation(Book book)
    {
        var canReserveResult = CanReserve(book);
        if (!canReserveResult.IsSuccess)
            return Result<Reservation>.Failure(canReserveResult.Errors);

        var reservation = Reservation.Create(this, book);
        if (!reservation.IsSuccess) return reservation;
        _reservations.Add(reservation.Value!);
        return reservation;
    }

    public void CancelReservation(Reservation reservation)
    {
        reservation.Cancel();
        _reservations.Remove(reservation);
    }

    public Result<Payment> PayFine(Fine fine)
    {
        if (!_fines.Contains(fine))
            return Result<Payment>.Failure("Fine does not belong to this member.");

        if (fine.Status != FineStatus.Unpaid)
            return Result<Payment>.Failure("Fine has already been paid.");

        var paymentResult = Payment.Create(this, fine.Amount, PaymentType.FinePayment);
        if (!paymentResult.IsSuccess)
            return paymentResult;

        _payments.Add(paymentResult.Value!);
        fine.Pay();
        paymentResult.Value!.Complete();

        return paymentResult;
    }

    public Result<Payment> PayMembershipFee(Money amount)
    {
        var paymentResult = Payment.Create(this, amount, PaymentType.MembershipFee);
        if (!paymentResult.IsSuccess)
            return paymentResult;

        _payments.Add(paymentResult.Value!);
        paymentResult.Value!.Complete();

        return paymentResult;
    }
}