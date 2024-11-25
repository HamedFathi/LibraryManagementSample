﻿using HamedStack.TheAggregateRoot;
using HamedStack.TheResult;
using LMS.Domain.BookContext.AggregateRoots;
using LMS.Domain.BookContext.Entities;
using LMS.Domain.MemberContext.AggregateRoots;
using LMS.Domain.ReservationContext.Enumerations;
using LMS.Domain.ReservationContext.ValueObjects;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace LMS.Domain.ReservationContext.Entities;

public class Reservation : Entity<Guid>
{
    public Book Book { get; }
    public BookCopy? Copy { get; private set; }
    public Member Member { get; }
    public ReservationDate ReservationDate { get; }
    public ExpiryDate ExpiryDate { get; }
    public ReservationStatus Status { get; private set; }

    private Reservation(Member member, Book book, ReservationDate reservationDate, ExpiryDate expiryDate)
    {
        Id = Guid.NewGuid();
        Member = member;
        Book = book;
        ReservationDate = reservationDate;
        ExpiryDate = expiryDate;
        Status = ReservationStatus.Pending;
    }

    public static Result<Reservation> Create(Member member, Book book)
    {
        var reservationDate = ReservationDate.Create(DateTime.Now);
        var expiryDate = ExpiryDate.Create(reservationDate.Value!, member.MembershipType.ReservationExpiryDays);

        return Result<Reservation>.Success(new Reservation(member, book, reservationDate.Value!, expiryDate.Value!));
    }

    public void Fulfill(BookCopy copy)
    {
        Copy = copy;
        Status = ReservationStatus.Fulfilled;
    }

    public void Cancel()
    {
        Status = ReservationStatus.Cancelled;
    }
}
