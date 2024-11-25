namespace LMS.Infrastructure.Models;

public class BookModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Isbn { get; set; }
    public string Publisher { get; set; }
    public int PublicationYear { get; set; }
    public string Edition { get; set; }
    public int Category { get; set; }
    public int MaxCopies { get; set; }
    public List<BookCopyModel> Copies { get; set; } = new();
    public List<BookAuthorModel> BookAuthors { get; set; } = new();
}