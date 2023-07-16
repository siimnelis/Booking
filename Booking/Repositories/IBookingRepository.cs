using Booking.Models;

namespace Booking.Repositories;

public interface IBookingRepository
{
    void Add(Models.Booking booking);
    Models.Booking GetBooking(int bookingId);
    List<Models.Booking> GetUserBookings(User user);
    public List<Models.Booking> GetBookingsForARoom(Room room);
}