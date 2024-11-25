using FluentAssertions;
using LMS.Application.Commands.Book.Create;

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
            Authors = new List<string>() { "test" }

        });

        output.IsSuccess.Should().BeTrue();
        output.Value.Should().NotBeEmpty();
    }
}