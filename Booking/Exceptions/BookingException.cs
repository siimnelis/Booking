namespace Booking.Exceptions;

public abstract class BookingException : Exception
{
    protected BookingException()
    {
        
    }
    
    protected BookingException(string message) : base(message)
    {
        
    }
}