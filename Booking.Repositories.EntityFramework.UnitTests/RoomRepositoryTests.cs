using Booking.Exceptions;
using Booking.Models;
using Booking.ValueTypes;
using FluentAssertions;
using Xunit;

namespace Booking.Repositories.EntityFramework.UnitTests;

public class RoomRepositoryTests
{
    [Fact]
    public void GetCheapestRoom_ReturnsTheCheapestAvailableRoom()
    {
        var bookingContext = new BookingContext();

        var cheapestAvailableRoom = new Room(2, 200, 2, new Money(5));
        var periodOfStay = new PeriodOfStay { Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 6)};
        
        bookingContext.Rooms.Add(new Room(1, 300, 3, new Money(10)));
        bookingContext.Rooms.Add(cheapestAvailableRoom);
        bookingContext.Rooms.Add(new Room(3, 300, 3, new Money(15)));
        bookingContext.Rooms.Add(new Room(4, 400, 3, new Money(4)));

        bookingContext.Bookings.Add(new Models.Booking(1, 4, 1, new Money(4),
            new PeriodOfStay{ Start = new DateTime(2023, 6, 30), End = new DateTime(2023, 7, 30) }));
        
        var roomsRepository = new RoomRepository(bookingContext, new BookingRepository(bookingContext));

        var room = roomsRepository.GetCheapestAvailableRoom(periodOfStay);

        room.Should().NotBeNull();
        room.Should().BeSameAs(cheapestAvailableRoom);
    }

    [Fact]
    public void GetCheapestRoom_ThrowsExceptionWhenNoAvailableRoomIsFound()
    {
        var bookingContext = new BookingContext();
        var periodOfStay = new PeriodOfStay { Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 6)};
        
        var roomsRepository = new RoomRepository(bookingContext, new BookingRepository(bookingContext));

        var act = () =>
        {
            roomsRepository.GetCheapestAvailableRoom(periodOfStay);
        };

        act.Should().Throw<RoomNotFoundException>();
    }
    
    [Fact]
    public void GetRoomById_ThrowsExceptionWhenNoRoomIsFound()
    {
        var bookingContext = new BookingContext();

        var roomsRepository = new RoomRepository(bookingContext, new BookingRepository(bookingContext));

        var act = () =>
        {
            roomsRepository.GetRoomById(1);
        };

        act.Should().Throw<RoomNotFoundException>();
    }
    
    [Fact]
    public void GetRoomById_ReturnRoomById()
    {
        var bookingContext = new BookingContext();

        var roomToBeFound = new Room(1, 300, 3, new Money(15));
        
        bookingContext.Rooms.Add(roomToBeFound);
        bookingContext.Rooms.Add(new Room(2, 400, 3, new Money(4)));

        var roomsRepository = new RoomRepository(bookingContext, new BookingRepository(bookingContext));

        var room = roomsRepository.GetRoomById(1);

        room.Should().NotBeNull();
        room.Should().BeSameAs(roomToBeFound);
    }

    [Fact]
    public void Add_CanAddRoom()
    {
        var bookingContext = new BookingContext();
        var addedRoom = new Room(1, 300, 3, new Money(15));
        
        var roomsRepository = new RoomRepository(bookingContext, new BookingRepository(bookingContext));

        roomsRepository.Add(addedRoom);

        var room = roomsRepository.GetRoomById(addedRoom.Id);

        room.Should().NotBeNull();
        room.Should().BeSameAs(addedRoom);
    }
    
    [Fact]
    public void GetRoomsAvailableForPeriodOfStay_ReturnsTheAvailableRooms()
    {
        var bookingContext = new BookingContext();
        
        var periodOfStay = new PeriodOfStay { Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 6)};

        var availableRooms = new List<Room>()
        {
            new (1, 100, 2, new Money(5)),
            new (2, 200, 3, new Money(15)),
            new (3, 300, 3, new Money(20))
        };

        foreach (var availableRoom in availableRooms)
            bookingContext.Rooms.Add(availableRoom);
        
        bookingContext.Rooms.Add(new Room(4, 400, 3, new Money(4)));

        bookingContext.Bookings.Add(new Models.Booking(1, 4, 1, new Money(4),
            new PeriodOfStay{ Start = new DateTime(2023, 6, 30), End = new DateTime(2023, 7, 30) }));

        var roomsRepository = new RoomRepository(bookingContext, new BookingRepository(bookingContext));

        var rooms = roomsRepository.GetRoomsAvailableForPeriodOfStay(periodOfStay);

        rooms.Should().NotBeNull();
            
        rooms.Should().BeEquivalentTo(availableRooms);
    }
    
}