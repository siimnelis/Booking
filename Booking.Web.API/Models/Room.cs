namespace Booking.Web.API.Models;

public class Room
{
    public required int Id { get; set; }
    public required int Number { get; set; }
    public required int NumberOfBedPlaces { get; set; }
    public required decimal Price { get; set; }
}