using Booking.Exceptions;
using Booking.ValueTypes;

namespace Booking.Models;

public class Booking
{
    internal Booking()
    {
        
    }
    
    public Booking(int id, int roomId, int userId, Money price, PeriodOfStay periodOfStay)
    {
        Id = id;
        RoomId = roomId;
        UserId = userId;
        Price = price;
        PeriodOfStay = periodOfStay;
    }
    
    public int Id { get; internal set; }
    public int RoomId { get; internal set; }
    public int UserId { get; internal set; }
    public Money Price { get; internal set; }
    public PeriodOfStay PeriodOfStay { get; internal set; }
    public BookingStatus Status { get; internal set; } = BookingStatus.Active;

    public void Cancel(IDateTime dateTime)
    {
        if (dateTime.Now.AddDays(3) >= PeriodOfStay.Start)
            throw new CantCancelBooking3DaysBeforePeriodOfStayException();
        
        Status = BookingStatus.Canceled;
    }
}