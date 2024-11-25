using HamedStack.TheAggregateRoot.Events;

namespace LMS.Domain.BookContext.DomainEvents
{
    public class BookInserted : DomainEvent
    {
        public Guid Id { get; set; }
    }
}
