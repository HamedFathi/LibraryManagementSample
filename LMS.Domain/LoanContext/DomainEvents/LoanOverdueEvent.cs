using HamedStack.TheAggregateRoot.Events;

namespace LMS.Domain.LoanContext.DomainEvents;

public class LoanOverdueEvent : DomainEvent
{
    public Guid LoanId { get; }
    public Guid MemberId { get; }
    public DateTime DueDate { get; }

    public LoanOverdueEvent(Guid loanId, Guid memberId, DateTime dueDate)
    {
        LoanId = loanId;
        MemberId = memberId;
        DueDate = dueDate;
    }
}