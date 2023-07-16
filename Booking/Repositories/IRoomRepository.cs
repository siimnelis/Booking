using Booking.Models;
using Booking.ValueTypes;

namespace Booking.Repositories;

public interface IRoomRepository
{
    Room GetRoomById(int roomId);
    Room GetCheapestAvailableRoom(PeriodOfStay periodOfStay);
    List<Room> GetRoomsAvailableForPeriodOfStay(PeriodOfStay periodOfStay);
    void Add(Room room);
}