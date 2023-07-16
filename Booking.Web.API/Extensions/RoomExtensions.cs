using Booking.Web.API.Models;

namespace Booking.Web.API.Extensions;

public static class RoomExtensions
{
    public static Room Map(this Booking.Models.Room room)
    {
        return new Room
        {
            Id = room.Id,
            Number = room.Number,
            NumberOfBedPlaces = room.NumberOfBedPlaces,
            Price = room.Price.Value
        };
    }

    public static IEnumerable<Room> Map(this IEnumerable<Booking.Models.Room> rooms)
    {
        return rooms.Select(room => room.Map());
    }
}