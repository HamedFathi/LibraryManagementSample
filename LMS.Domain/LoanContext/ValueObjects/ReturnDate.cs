using HamedStack.TheResult;
using LMS.Domain.SharedKernel.ValueObjects;

namespace LMS.Domain.LoanContext.ValueObjects;

public class ReturnDate : DateValueObject
{
    private ReturnDate(DateTime value) : base(value) { }

    public static Result<ReturnDate> Create(DateTime value)
    {
        if (value > DateTime.Now)
            return Result<ReturnDate>.Failure("Return date cannot be in the future.");
        return Result<ReturnDate>.Success(new ReturnDate(value));
    }
}