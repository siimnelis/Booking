namespace Booking.Web.API.Models;

public class Booking
{
    public required int Id { get; set; }
    public required int RoomId { get; set; }
    public required int UserId { get; set; }
    public required decimal Price { get; set; }
    public required PeriodOfStay PeriodOfStay { get; set; }
    public required BookingStatus Status { get; set; }
}