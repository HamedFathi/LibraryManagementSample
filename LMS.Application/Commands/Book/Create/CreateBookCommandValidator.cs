using FluentValidation;
using HamedStack.CQRS.FluentValidation;

namespace LMS.Application.Commands.Book.Create;

public class CreateBookCommandValidator : CommandValidator<CreateBookCommand, Guid>
{
    public CreateBookCommandValidator()
    {
        RuleFor(b => b.Title).Length(1, 100);
    }
}