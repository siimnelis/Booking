using Booking.Web.API.Models;

namespace Booking.Web.API.Extensions;

public static class BookingExtensions
{
    public static Models.Booking Map(this Booking.Models.Booking booking)
    {
        return new Models.Booking
        {
            Id = booking.Id,
            PeriodOfStay = booking.PeriodOfStay.Map(),
            Price = booking.Price.Value,
            RoomId = booking.RoomId,
            UserId = booking.UserId,
            Status = (BookingStatus)booking.Status
        };
    }

    public static IEnumerable<Models.Booking> Map(this IEnumerable<Booking.Models.Booking> bookings)
    {
        return bookings.Select(booking => booking.Map());
    }
}