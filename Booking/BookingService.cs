using Booking.Exceptions;
using Booking.Extensions;
using Booking.Repositories;
using Booking.ValueTypes;

namespace Booking;

public class BookingService : IBookingService
{
    private IBookingRepository BookingRepository { get; }
    private IRoomRepository RoomRepository { get; }
    private IUserRepository UserRepository { get; }
    private IIdsGenerator IdGenerator { get; }
    private UserContext UserContext { get; }
    private IDateTime DateTime { get; }
    
    public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository, IUserRepository userRepository, IIdsGenerator idGenerator, UserContext userContext, IDateTime dateTime)
    {
        BookingRepository = bookingRepository;
        RoomRepository = roomRepository;
        UserRepository = userRepository;
        IdGenerator = idGenerator;
        UserContext = userContext;
        DateTime = dateTime;
    }
    
    public Models.Booking BookRoom(int roomId, PeriodOfStay periodOfStay)
    {
        var room = RoomRepository.GetRoomById(roomId);

        var bookingsForARoom = BookingRepository.GetBookingsForARoom(room);

        if (bookingsForARoom.AnyOverlapingBookingsForARoom(periodOfStay))
            throw new RoomAlreadyBookedForPeriodOfStayException();

        var user = UserRepository.GetUserById(UserContext.UserId);
        
        var booking = new Models.Booking
        {
            Id = IdGenerator.GenerateBookingId(),
            PeriodOfStay = periodOfStay,
            Price = new Money(room.Price.Value * periodOfStay.NumberOfDays),
            RoomId = room.Id,
            UserId = user.Id
        };
        
        BookingRepository.Add(booking);

        return booking;
    }

    public Models.Booking BookCheapestRoom(PeriodOfStay periodOfStay)
    {
        var room = RoomRepository.GetCheapestAvailableRoom(periodOfStay);

        return BookRoom(room.Id, periodOfStay);
    }

    public List<Models.Booking> GetUserBookings()
    {
        var user = UserRepository.GetUserById(UserContext.UserId);
        
        return BookingRepository.GetUserBookings(user);
    }

    public List<Models.Booking> GetBookingsForARoom(int roomId)
    {
        var user = UserRepository.GetUserById(UserContext.UserId);

        if (user.Role != Role.Staff)
            throw new UnAuthorizedAccessException();

        var room = RoomRepository.GetRoomById(roomId);
        
        return BookingRepository.GetBookingsForARoom(room);
    }

    public void CancelBooking(int bookingId)
    {
        var user = UserRepository.GetUserById(UserContext.UserId);

        var booking = BookingRepository.GetBooking(bookingId);

        if (!(user.Role == Role.Staff || booking.UserId == UserContext.UserId))
            throw new UnAuthorizedAccessException();

        booking.Cancel(DateTime);
    }
}