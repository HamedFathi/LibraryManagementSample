namespace LMS.WebApi.Endpoints.Book;

public class BookRequest
{
    public string Title { get; set; }
    public IEnumerable<AuthorRequest> Authors { get; set; }
    public string Isbn { get; set; }
    public string Publisher { get; set; }
    public int PublicationYear { get; set; }
    public string Edition { get; set; }
    public int Category { get; set; }
    public int MaxCopies { get; set; }
}

public class AuthorRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Biography { get; set; }
}