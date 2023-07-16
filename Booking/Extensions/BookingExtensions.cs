using Booking.ValueTypes;

namespace Booking.Extensions;

public static class BookingExtensions
{
    public static bool AnyOverlapingBookingsForARoom(this List<Models.Booking> bookingsForARoom, PeriodOfStay periodOfStay)
    {
        return bookingsForARoom.Where(booking => booking.Status == BookingStatus.Active)
            .Any(booking => periodOfStay.OverlapsWith(booking.PeriodOfStay));
    }
}