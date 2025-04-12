using Asp.Versioning;
using HamedStack.AspNetCore.Endpoint;
using HamedStack.CQRS;
using HamedStack.TheResult.AspNetCore;
using LMS.Application.Commands.Book.Create;
using LMS.Application.DTOs;

namespace LMS.WebApi.Endpoints.Book;

public class CreateBookEndpoint : MinimalApiEndpointBase
{
    public override void HandleEndpoint(IEndpointRouteBuilder endpoint)
    {
        var apiVersionSet = Application.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        endpoint.MapPost("/v{version:apiVersion}/book", CreateBookEndpointHandler)
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
            })
            .WithApiVersionSet(apiVersionSet)
            .MapToApiVersion(1)
            ;
    }

    private static async Task<IResult> CreateBookEndpointHandler(BookRequest request, ICommandQueryDispatcher dispatcher)
    {
        var output = await dispatcher.Send(new CreateBookCommand()
        {
            Authors = request.Authors.Select(a => new AuthorDto()
            {
                FirstName = a.FirstName,
                LastName = a.LastName,
                Biography = a.Biography
            }),
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