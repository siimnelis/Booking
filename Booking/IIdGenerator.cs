namespace Booking;

public interface IIdsGenerator
{
    int GenerateBookingId();
    int GenerateRoomId();
    int GenerateUserId();
}