namespace LMS.WebApi.Controllers;


// SEE Endpoints!

/*
[ApiController]
[Microsoft.AspNetCore.Mvc.Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly ICommandQueryDispatcher _dispatcher;

    public BookController(ICommandQueryDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpPost]
    public async Task<IActionResult> Post(BookRequest request)
    {
        var output = await _dispatcher.Send(new CreateBookCommand()
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
        return output.ToActionResult();
    }
}
*/