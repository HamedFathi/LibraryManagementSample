using FluentValidation;
using FluentValidation.Results;
using LMS.Application.Commands.Book.Create;
using LMS.Application.DTOs;
using LMS.Domain.BookContext.AggregateRoots;
using LMS.Domain.BookContext.Entities;
using LMS.Domain.BookContext.Repositories;
using NSubstitute;

namespace LMS.UnitTests;

public class MyUnitTests
{
    [Fact]
    public async Task CreateBookCommandHandler_ShouldCallAddAsync()
    {
        var mockRepo = Substitute.For<IBookRepository>();

        var authors = new List<Author>()
        {
            Author.Create("author_first","author_last","")!
        };
        var book =
            Book.Create("test", authors, "978-1-56619-909-4", "test", 2000, 2, 10, "");
        
        mockRepo.AddAsync(Arg.Any<Book>(), CancellationToken.None)
            .Returns(Task.FromResult(book.Value)!);

        var mockValidator = Substitute.For<IValidator<CreateBookCommand>>();
        mockValidator.ValidateAsync(Arg.Any<CreateBookCommand>(), CancellationToken.None)
            .Returns(Task.FromResult(new ValidationResult()));

        var handler = new CreateBookCommandHandler(mockRepo, mockValidator);
        var command = new CreateBookCommand()
        {
            Title = "test",
            Isbn = "978-1-56619-909-4",
            Publisher = "test",
            PublicationYear = 2009,
            Edition = "",
            Category = 3,
            MaxCopies = 10,
            Authors = new List<AuthorDto>()
            {
                new()
                {
                    FirstName = "test_name",
                    LastName = "test_last"
                }
            }

        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);

        await mockValidator.Received(1).ValidateAsync(Arg.Any<CreateBookCommand>(), CancellationToken.None);
        await mockRepo.Received(1).AddAsync(Arg.Any<Book>());
    }
}