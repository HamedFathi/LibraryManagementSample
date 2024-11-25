using HamedStack.CQRS;

namespace LMS.Application.Commands.Book.Create;

public class CreateBookCommand : ICommand<Guid>
{
    public string Title { get; set; }
    public IEnumerable<string> Authors { get; set; }
    public string Isbn { get; set; }
    public string Publisher { get; set; }
    public int PublicationYear { get; set; }
    public string Edition { get; set; }
    public int Category { get; set; }
    public int MaxCopies { get; set; }
}