using HamedStack.TheResult;
using LMS.Domain.SharedKernel.ValueObjects;

namespace LMS.Domain.ReservationContext.ValueObjects;

public class ReservationDate : DateValueObject
{
    private ReservationDate(DateTime value) : base(value) { }

    public static Result<ReservationDate> Create(DateTime value)
    {
        return Result<ReservationDate>.Success(new ReservationDate(value));
    }
}