namespace LMS.Infrastructure.Models;

public class BookAuthorModel
{
    public Guid BookId { get; set; }
    public BookModel Book { get; set; }
    public Guid AuthorId { get; set; }
    public AuthorModel Author { get; set; }
}