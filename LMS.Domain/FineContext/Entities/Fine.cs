using HamedStack.TheAggregateRoot;
using HamedStack.TheResult;
using LMS.Domain.FineContext.Enumerations;
using LMS.Domain.LoanContext.Entities;
using LMS.Domain.MemberContext.AggregateRoots;
using LMS.Domain.SharedKernel.ValueObjects;


// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace LMS.Domain.FineContext.Entities;

public class Fine : Entity<Guid>
{
    public Loan Loan { get; }
    public Member Member { get; }
    public Money Amount { get; }
    public DateTime DateIssued { get; }
    public FineStatus Status { get; private set; }

    private Fine(Loan loan, Money amount)
    {
        Id = Guid.NewGuid();
        Loan = loan;
        Member = loan.Member;
        Amount = amount;
        DateIssued = DateTime.Now;
        Status = FineStatus.Unpaid;
    }

    public static Result<Fine> Create(Loan loan, Money amount)
    {
        if (loan == null)
            return Result<Fine>.Failure("Loan cannot be null.");

        return Result<Fine>.Success(new Fine(loan, amount));
    }

    public void Pay()
    {
        Status = FineStatus.Paid;
    }
}
