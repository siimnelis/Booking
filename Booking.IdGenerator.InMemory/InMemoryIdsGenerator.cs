namespace Booking.IdGenerator.InMemory;

public class InMemoryIdsGenerator : IIdsGenerator
{
    private int _bookingId;
    private int _roomId;
    private int _userId;
    
    public int GenerateBookingId()
    {
        return ++_bookingId;
    }

    public int GenerateRoomId()
    {
        return ++_roomId;
    }

    public int GenerateUserId()
    {
        return ++_userId;
    }
}