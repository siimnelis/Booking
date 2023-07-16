namespace Booking;

public class SystemDateTime : IDateTime
{
    public DateTime Now => DateTime.Now;
}