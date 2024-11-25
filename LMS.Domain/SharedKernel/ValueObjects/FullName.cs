using HamedStack.TheAggregateRoot;
using HamedStack.TheResult;

namespace LMS.Domain.SharedKernel.ValueObjects;

public class FullName : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }

    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static Result<FullName> Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result<FullName>.Failure("First name cannot be empty.");

        if (string.IsNullOrWhiteSpace(lastName))
            return Result<FullName>.Failure("Last name cannot be empty.");

        return Result<FullName>.Success(new FullName(firstName, lastName));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}