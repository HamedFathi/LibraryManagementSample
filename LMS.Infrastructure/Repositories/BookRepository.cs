using HamedStack.TheRepository.EntityFrameworkCore;
using LMS.Domain.BookContext.AggregateRoots;
using HamedStack.TheAggregateRoot.Abstractions;
using LMS.Domain.BookContext.Repositories;
using LMS.Infrastructure.Models;

namespace LMS.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly DbContextBase _dbContext;
    private readonly TimeProvider _timeProvider;

    public BookRepository(DbContextBase dbContext, TimeProvider timeProvider)
    {
        _dbContext = dbContext;
        _timeProvider = timeProvider;
    }

    public virtual async Task<Book> AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        if (book == null) throw new ArgumentNullException(nameof(book));
        ApplyCreatedAuditTime(book);

        var bookDb = new BookModel()
        {
            Id = book.Id,
            Category = book.Category,
            Edition = book.PublicationInfo.Edition,
            Isbn = book.Isbn,
            PublicationYear = book.PublicationInfo.PublicationYear,
            Title = book.Title,
            Publisher = book.PublicationInfo.Publisher,
            MaxCopies = book.MaxCopies,
            Copies = book.Copies.Select(c => new BookCopyModel()
            {
                Id = c.Id,
                Barcode = c.Barcode,
                BookId = book.Id,
                Condition = c.Condition,
                Status = c.CurrentStatus
            }).ToList(),
            BookAuthors = book.Authors.List.Select(a => new BookAuthorModel()
            {
                BookId = book.Id,
                AuthorId = Guid.NewGuid(),
                Author = new AuthorModel()
                {
                    Id = Guid.NewGuid(),
                    Name = a.Value
                }
            }).ToList()
        };

        await _dbContext.AddAsync(bookDb, cancellationToken);
        _dbContext.AddDomainEvents(book.DomainEvents);

        await _dbContext.SaveChangesAsync(cancellationToken);
        return book;
    }

    protected virtual void ApplyCreatedAuditTime(object entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        if (entity is IAudit audit)
        {
            audit.CreatedOn = _timeProvider.GetUtcNow();
            audit.CreatedBy = ToString();
        }
    }
    protected virtual void ApplyModifiedAuditTime(object entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        if (entity is IAudit audit)
        {
            audit.ModifiedOn = _timeProvider.GetUtcNow();
            audit.ModifiedBy = ToString();
        }
    }
}