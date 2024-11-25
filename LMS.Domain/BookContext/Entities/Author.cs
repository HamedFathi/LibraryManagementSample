using HamedStack.TheAggregateRoot;
using HamedStack.TheResult;

namespace LMS.Domain.BookContext.Entities;

public class Author : Entity<Guid>
{
    public string FirstName { get; }
    public string LastName { get; }
    public string? Biography { get;  }

    private Author(string firstName, string lastName, string? biography = null)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Biography = biography;
    }

    public static Result<Author> Create(string firstName, string lastName, string? biography = null)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            return Result<Author>.Failure("Author's first name and last name are required.");

        return Result<Author>.Success(new Author(firstName, lastName, biography));
    }

    public override string ToString() => $"{FirstName} {LastName}";
}