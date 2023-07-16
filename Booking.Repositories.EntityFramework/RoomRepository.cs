using Booking.Exceptions;
using Booking.Extensions;
using Booking.Models;
using Booking.ValueTypes;

namespace Booking.Repositories.EntityFramework;

public class RoomRepository : IRoomRepository
{
    private BookingContext BookingContext { get; }
    private IBookingRepository BookingRepository { get; }
    
    public RoomRepository(BookingContext bookingContext, IBookingRepository bookingRepository)
    {
        BookingContext = bookingContext;
        BookingRepository = bookingRepository;
    }
    public Room GetRoomById(int roomId)
    {
        var room = BookingContext.Rooms.Local.FirstOrDefault(room => room.Id == roomId);

        if (room == null)
            throw new RoomNotFoundException();
        
        return room;
    }

    public Room GetCheapestAvailableRoom(PeriodOfStay periodOfStay)
    {
        var room = GetRoomsAvailableForPeriodOfStay(periodOfStay).MinBy(x => x.Price.Value);

        if (room == null)
            throw new RoomNotFoundException();

        return room;
    }

    public List<Room> GetRoomsAvailableForPeriodOfStay(PeriodOfStay periodOfStay)
    {
        var availableRooms = new List<Room>();

        foreach (var room in BookingContext.Rooms.Local)
        {
            var bookings = BookingRepository.GetBookingsForARoom(room);

            if (!bookings.AnyOverlapingBookingsForARoom(periodOfStay))
            {
                availableRooms.Add(room);
            }   
        }

        return availableRooms;
    }

    public void Add(Room room)
    {
        BookingContext.Rooms.Add(room);
    }
}