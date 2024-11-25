using FluentValidation;
using HamedStack.CQRS;
using HamedStack.TheResult;
using HamedStack.TheResult.FluentValidation;
using LMS.Domain.BookContext.DomainEvents;
using LMS.Domain.BookContext.Repositories;

namespace LMS.Application.Commands.Book.Create;

public class CreateBookCommandHandler : ICommandHandler<CreateBookCommand, Guid>
{
    private readonly IBookRepository _repository;
    private readonly IValidator<CreateBookCommand> _validator;

    public CreateBookCommandHandler(IBookRepository repository, IValidator<CreateBookCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    public async Task<Result<Guid>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToResult<Guid>();
        }

        var book =
            Domain.BookContext.AggregateRoots.Book.Create(request.Title, request.Authors, request.Isbn!,
                request.Publisher, request.PublicationYear, request.Category, request.MaxCopies, request.Edition);
        if (!book.IsSuccess)
        {
            return Result<Guid>.Failure(Guid.Empty, book.Errors);
        }

        book.Value?.AddDomainEvent(new BookInserted()
        {
            Id = book.Value.Id
        });

        var output = await _repository.AddAsync(book.Value!, cancellationToken);
        return Result<Guid>.Success(output.Id);
    }
}