using Booking.Models;
using Booking.ValueTypes;

namespace Booking;

public interface IRoomsService
{
    List<Room> GetRoomsAvailableForPeriodOfStay(PeriodOfStay periodOfStay);
}