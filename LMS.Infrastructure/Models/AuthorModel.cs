
namespace LMS.Infrastructure.Models;

public class AuthorModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<BookAuthorModel> BookAuthors { get; set; } = new();
}