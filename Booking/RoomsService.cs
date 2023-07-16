using Booking.Models;
using Booking.Repositories;
using Booking.ValueTypes;

namespace Booking;

public class RoomsService : IRoomsService
{
    private IRoomRepository RoomRepository { get; }
    
    public RoomsService(IRoomRepository roomRepository)
    {
        RoomRepository = roomRepository;
    }
    
    public List<Room> GetRoomsAvailableForPeriodOfStay(PeriodOfStay periodOfStay)
    {
        return RoomRepository.GetRoomsAvailableForPeriodOfStay(periodOfStay);
    }
}