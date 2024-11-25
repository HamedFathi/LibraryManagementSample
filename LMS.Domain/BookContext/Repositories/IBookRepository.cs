using LMS.Domain.BookContext.AggregateRoots;

namespace LMS.Domain.BookContext.Repositories;

public interface IBookRepository
{
    Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default);
}
