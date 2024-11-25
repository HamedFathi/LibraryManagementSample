﻿using HamedStack.TheAggregateRoot;
using HamedStack.TheResult;

namespace LMS.Domain.BookContext.ValueObjects;

public class Author : SingleValueObject<string>
{
    private Author(string value) : base(value) { }

    public static Result<Author> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<Author>.Failure("Author name cannot be empty.");
        return Result<Author>.Success(new Author(value));
    }
}