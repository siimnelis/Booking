using Booking.ValueTypes;

namespace Booking.Exceptions;

public class PeriodOfStayMustBeAtleastForADayException : BookingException
{
    public PeriodOfStayMustBeAtleastForADayException() : base("Period of stay must be for at least a day.")
    {
        
    }
}