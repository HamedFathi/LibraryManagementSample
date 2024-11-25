using FluentAssertions;
using LMS.Application.Commands.Book.Create;
using LMS.Application.DTOs;
using LMS.Domain.BookContext.Entities;

namespace LMS.IntegrationTests;

public class MyIntegrationTests : WebIntegrationTestBase
{
    public MyIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task ShouldReturnSuccessWhenBookIsCreated()
    {
        var output = await Dispatcher.Send(new CreateBookCommand()
        {
            Title = "test",
            Isbn = "978-1-56619-909-4",
            Publisher = "test",
            PublicationYear = 2009,
            Edition = "",
            Category = 3,
            MaxCopies = 10,
            Authors = new List<AuthorDto>() { new AuthorDto()
            {
                FirstName = "test_name",
                LastName = "test_last"
            } }

        });

        output.IsSuccess.Should().BeTrue();
        output.Value.Should().NotBeEmpty();
    }
}