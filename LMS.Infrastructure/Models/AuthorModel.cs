
namespace LMS.Infrastructure.Models;

public class AuthorModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Biography { get; set; }
    public List<BookAuthorModel> BookAuthors { get; set; } = new();
}