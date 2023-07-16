using Booking.Exceptions;
using Booking.Models;

namespace Booking.Repositories.EntityFramework;

public class BookingRepository : IBookingRepository
{
    private BookingContext BookingContext { get; }
    
    public BookingRepository(BookingContext bookingContext)
    {
        BookingContext = bookingContext;
    }
    
    public void Add(Models.Booking booking)
    {
        BookingContext.Add(booking);
    }

    public Models.Booking GetBooking(int bookingId)
    {
        var booking = BookingContext.Bookings.Local.FirstOrDefault(x => x.Id == bookingId);

        if (booking == null)
            throw new NoBookingFoundException();
        
        return booking;
    }

    public List<Models.Booking> GetUserBookings(User user)
    {
        return BookingContext.Bookings.Local.Where(x => x.UserId == user.Id).ToList();
    }

    public List<Models.Booking> GetBookingsForARoom(Room room)
    {
        return BookingContext.Bookings.Local.Where(x => x.RoomId == room.Id).ToList();
    }
}