using Booking.ValueTypes;

namespace Booking.Web.API.Extensions;

public static class PeriodOfStayExtensions
{
    public static PeriodOfStay Map(this Models.PeriodOfStay periodOfStay, IDateTime dateTime)
    {
        return new PeriodOfStay(periodOfStay.Start, periodOfStay.End, dateTime);
    }

    public static Models.PeriodOfStay Map(this PeriodOfStay periodOfStay)
    {
        return new Models.PeriodOfStay
        {
            Start = periodOfStay.Start,
            End = periodOfStay.End
        };
    }
}