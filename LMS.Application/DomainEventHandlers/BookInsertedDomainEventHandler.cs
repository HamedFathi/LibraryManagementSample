using HamedStack.CQRS;
using LMS.Domain.BookContext.DomainEvents;

namespace LMS.Application.DomainEventHandlers
{
    public class BookInsertedDomainEventHandler : IDomainEventHandler<BookInserted>
    {
        public Task Handle(BookInserted notification, CancellationToken cancellationToken)
        {
            

            return Task.CompletedTask;
        }
    }
}
