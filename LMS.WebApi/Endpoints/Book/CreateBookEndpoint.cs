using HamedStack.AspNetCore.Endpoint;
using HamedStack.CQRS;
using HamedStack.TheResult.AspNetCore;
using LMS.Application.Commands.Book.Create;

namespace LMS.WebApi.Endpoints.Book;

public class CreateBookEndpoint : IMinimalApiEndpoint
{
    public void HandleEndpoint(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost("/book", CreateBookEndpointHandler)
            .WithTags("Book")
            .WithDescription("Creates a new book in the system.")
            //.Produces(StatusCodes.Status201Created) 
            //.ProducesValidationProblem() 
            //.Produces<ProblemDetails>(StatusCodes.Status500InternalServerError) 
            .Accepts<BookRequest>("application/json") 
            .WithName("CreateBook") 
            .WithOpenApi(operation =>
            {
                operation.Summary = "Create a new book";
                operation.Description = "Creates a new book in the library catalog using the provided details.";
                operation.OperationId = "CreateBook"; 
                //operation.Responses["201"].Description = "Book created successfully.";
                //operation.Responses["400"].Description = "Invalid request data.";
                //operation.Responses["500"].Description = "An unexpected error occurred.";
                return operation;
            });
    }

    private static async Task<IResult> CreateBookEndpointHandler(BookRequest request, ICommandQueryDispatcher dispatcher)
    {
        var output = await dispatcher.Send(new CreateBookCommand()
        {
            Authors = request.Authors,
            Category = request.Category,
            Edition = request.Edition,
            Isbn = request.Isbn,
            MaxCopies = request.MaxCopies,
            PublicationYear = request.PublicationYear,
            Publisher = request.Publisher,
            Title = request.Title
        });
        return output.ToMinimalApiResult();
    }
}