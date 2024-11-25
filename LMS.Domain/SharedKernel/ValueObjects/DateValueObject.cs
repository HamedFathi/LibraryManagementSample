﻿using HamedStack.TheAggregateRoot;

namespace LMS.Domain.SharedKernel.ValueObjects;

public abstract class DateValueObject : SingleValueObject<DateTime>
{
    protected DateValueObject(DateTime value) : base(value)
    {
        if (value == DateTime.MinValue)
            throw new ArgumentException("Date cannot be empty.");
    }

    public override string ToString()
    {
        return Value.ToShortDateString();
    }
}