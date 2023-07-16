using Booking.ValueTypes;

namespace Booking;

public interface IBookingService
{
    Models.Booking BookRoom(int roomId, PeriodOfStay periodOfStay);
    Models.Booking BookCheapestRoom(PeriodOfStay periodOfStay);
    List<Models.Booking> GetUserBookings();
    List<Models.Booking> GetBookingsForARoom(int roomId);
    void CancelBooking(int bookingId);
}