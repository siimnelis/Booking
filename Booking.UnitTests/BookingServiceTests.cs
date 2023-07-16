using Booking.Exceptions;
using Booking.Models;
using Booking.Repositories;
using Booking.ValueTypes;
using FluentAssertions;
using Moq;
using Xunit;

namespace Booking.UnitTests;

public class BookingServiceTests
{
    [Fact]
    public void BookRoom_CreatesANewBooking()
    {
        var periodOfStay = new PeriodOfStay {Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 7)};
    
            var bookingRepositoryMock = new Mock<IBookingRepository>();
        var roomRepositoryMock = new Mock<IRoomRepository>();
        var userRepositoryMock= new Mock<IUserRepository>();
        var idsGeneratorMock = new Mock<IIdsGenerator>();
        
        var room = new Room
        {
            Id = 1,
            Price = new Money(10),
            NumberOfBedPlaces = 1,
            Number = 1
        };
    
        roomRepositoryMock.Setup(x => x.GetRoomById(1)).Returns(room);

        var user = new User
        {
            Id = 1,
            FirstName = "",
            LastName = "",
            IdCode = "",
            EMail = new EMail(),
            Role = Role.Customer
        };
    
        userRepositoryMock.Setup(x => x.GetUserById(1)).Returns(user);
    
        Models.Booking bookingAddedToRepository = null!;
    
        bookingRepositoryMock.Setup(x => x.Add(It.IsAny<Models.Booking>()))
            .Callback<Models.Booking>(x => bookingAddedToRepository = x);
        bookingRepositoryMock.Setup(x => x.GetBookingsForARoom(room)).Returns(new List<Models.Booking>());
        
        idsGeneratorMock.Setup(x => x.GenerateBookingId()).Returns(1);
        
        var bookingService = new BookingService(bookingRepositoryMock.Object, roomRepositoryMock.Object,
            userRepositoryMock.Object, idsGeneratorMock.Object, new UserContext { UserId = 1 }, new Mock<IDateTime>().Object);
    
        var booking = bookingService.BookRoom(1, periodOfStay);

        booking.Should().BeSameAs(bookingAddedToRepository);
        bookingRepositoryMock.Verify(x => x.Add(It.IsAny<Models.Booking>()), Times.Once);
        booking.Should().NotBeNull();
        booking.Id.Should().Be(1);
        booking.RoomId.Should().Be(1);
        booking.UserId.Should().Be(1);
        booking.Price.Should().NotBeNull();
        booking.Price.Value.Should().Be(20);
        booking.Status.Should().Be(BookingStatus.Active);
        booking.PeriodOfStay.Should().NotBeNull();
        booking.PeriodOfStay.Should().Be(periodOfStay);
    }
    
    [Fact]
    public void BookCheapestRoom_CreatesANewBookingForCheapestRoom()
    {
        var periodOfStay = new PeriodOfStay {Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 6)};
        
        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var roomRepositoryMock = new Mock<IRoomRepository>();
        var userRepositoryMock = new Mock<IUserRepository>();
        var idsGeneratorMock = new Mock<IIdsGenerator>();
        
        var room = new Room
        {
            Id = 1,
            Price = new Money(10),
            NumberOfBedPlaces = 1,
            Number = 1
        };
    
        roomRepositoryMock.Setup(x => x.GetRoomById(1)).Returns(room);
        roomRepositoryMock.Setup(x => x.GetCheapestAvailableRoom(periodOfStay)).Returns(room);

        var user = new User
        {
            Id = 1,
            FirstName = "",
            LastName = "",
            IdCode = "",
            EMail = new EMail(),
            Role = Role.Customer
        };
    
        userRepositoryMock.Setup(x => x.GetUserById(1)).Returns(user);
    
        Models.Booking bookingAddedToRepository = null!;
    
        bookingRepositoryMock.Setup(x => x.Add(It.IsAny<Models.Booking>()))
            .Callback<Models.Booking>(x => bookingAddedToRepository = x);
        bookingRepositoryMock.Setup(x => x.GetBookingsForARoom(room)).Returns(new List<Models.Booking>());
        
        idsGeneratorMock.Setup(x => x.GenerateBookingId()).Returns(1);
        
        var bookingService = new BookingService(bookingRepositoryMock.Object, roomRepositoryMock.Object,
            userRepositoryMock.Object, idsGeneratorMock.Object, new UserContext { UserId = 1 }, new Mock<IDateTime>().Object);
    
        var booking = bookingService.BookCheapestRoom(periodOfStay);

        roomRepositoryMock.Verify(x => x.GetCheapestAvailableRoom(periodOfStay), Times.Once);
        booking.Should().BeSameAs(bookingAddedToRepository);
        bookingRepositoryMock.Verify(x => x.Add(It.IsAny<Models.Booking>()), Times.Once);
        booking.Should().NotBeNull();
        booking.Id.Should().Be(1);
        booking.RoomId.Should().Be(1);
        booking.UserId.Should().Be(1);
        booking.Price.Should().NotBeNull();
        booking.Price.Value.Should().Be(10);
        booking.Status.Should().Be(BookingStatus.Active);
        booking.PeriodOfStay.Should().NotBeNull();
        booking.PeriodOfStay.Should().Be(periodOfStay);
    }

    [Fact]
    public void GetUserBookings_ReturnsUserBookings()
    {
        var userContext = new UserContext { UserId = 1 };

        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var userRepositoryMock = new Mock<IUserRepository>();
        
        var user = new User
        {
            Id = 1,
            FirstName = "",
            LastName = "",
            IdCode = "",
            EMail = new EMail(),
            Role = Role.Customer
        };

        userRepositoryMock.Setup(x => x.GetUserById(userContext.UserId)).Returns(user);

        var expectedBookings = new List<Models.Booking>()
        {
            new()
            {
                Id = 1,
                PeriodOfStay = new PeriodOfStay {Start = new DateTime(2024, 7, 5), End = new DateTime(2024, 7, 6)},
                Price = new Money(),
                RoomId = 1,
                UserId = 1,
            }
        };

        bookingRepositoryMock.Setup(x => x.GetUserBookings(user)).Returns(expectedBookings);
        
        var bookingService = new BookingService(bookingRepositoryMock.Object, new Mock<IRoomRepository>().Object,
            userRepositoryMock.Object, new Mock<IIdsGenerator>().Object, userContext, new Mock<IDateTime>().Object);

        var bookings = bookingService.GetUserBookings();

        bookings.Should().NotBeNull();
        bookings.Should().BeSameAs(expectedBookings);
    }

    [Fact]
    public void GetBookingsForARoom_CustomersCantGetAllTheBookingsForARoom()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        
        var userContext = new UserContext { UserId = 1 };
        
        var user = new User
        {
            Id = 1,
            FirstName = "",
            LastName = "",
            IdCode = "",
            EMail = new EMail(),
            Role = Role.Customer
        };

        userRepositoryMock.Setup(x => x.GetUserById(userContext.UserId)).Returns(user);
        
        var bookingService = new BookingService(new Mock<IBookingRepository>().Object, new Mock<IRoomRepository>().Object,
            userRepositoryMock.Object, new Mock<IIdsGenerator>().Object, userContext, new Mock<IDateTime>().Object);

        var act = () =>
        {
            bookingService.GetBookingsForARoom(1);
        };

        act.Should().Throw<UnAuthorizedAccessException>();
    }

    [Fact]
    public void GetBookingsForARoom_ReturnsBookingsForARoom()
    {
        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var roomRepositoryMock = new Mock<IRoomRepository>();
        var userRepositoryMock = new Mock<IUserRepository>();
        
        var userContext = new UserContext { UserId = 1 };
        
        var user = new User
        {
            Id = 1,
            FirstName = "",
            LastName = "",
            IdCode = "",
            EMail = new EMail(),
            Role = Role.Staff
        };

        var room = new Room
        {
            Id = 1,
            Price = new Money(10),
            NumberOfBedPlaces = 1,
            Number = 1
        };
    
        var expectedBookings = new List<Models.Booking>()
        {
            new()
            {
                Id = 1,
                PeriodOfStay = new PeriodOfStay {Start = new DateTime(2024, 7, 5), End = new DateTime(2024, 7, 6)},
                Price = new Money(),
                RoomId = 1,
                UserId = 1,
            }
        };
        
        roomRepositoryMock.Setup(x => x.GetRoomById(room.Id)).Returns(room);
        userRepositoryMock.Setup(x => x.GetUserById(userContext.UserId)).Returns(user);
        bookingRepositoryMock.Setup(x => x.GetBookingsForARoom(room)).Returns(expectedBookings);
        
        
        var bookingService = new BookingService(bookingRepositoryMock.Object, roomRepositoryMock.Object,
            userRepositoryMock.Object, new Mock<IIdsGenerator>().Object, userContext, new Mock<IDateTime>().Object);

        var bookings = bookingService.GetBookingsForARoom(room.Id);

        bookings.Should().NotBeNull();
        bookings.Should().BeSameAs(expectedBookings);
    }

    [Fact]
    public void CancelBooking_CustomerCantCancelOtherUsersBookings()
    {
        var userContext = new UserContext { UserId = 1 };
        
        var user = new User
        {
            Id = 1,
            FirstName = "",
            LastName = "",
            IdCode = "",
            EMail = new EMail(),
            Role = Role.Customer
        };

        var booking = new Models.Booking
        {
            Id = 1,
            PeriodOfStay = new PeriodOfStay {Start = new DateTime(2024, 7, 5), End = new DateTime(2024, 7, 6)},
            Price = new Money(),
            RoomId = 1,
            UserId = 2
        };
        
        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var userRepositoryMock = new Mock<IUserRepository>();

        bookingRepositoryMock.Setup(x => x.GetBooking(booking.Id)).Returns(booking);
        userRepositoryMock.Setup(x => x.GetUserById(user.Id)).Returns(user);
        
        var bookingService = new BookingService(bookingRepositoryMock.Object, new Mock<IRoomRepository>().Object,
            userRepositoryMock.Object, new Mock<IIdsGenerator>().Object, userContext, new Mock<IDateTime>().Object);

        var act = () =>
        {
            bookingService.CancelBooking(booking.Id);
        };

        act.Should().Throw<UnAuthorizedAccessException>();
    }

    [Fact]
    public void CancelBooking_CustomerCanCancelTheirBooking()
    {
        var userContext = new UserContext { UserId = 1 };
        
        var user = new User
        {
            Id = 1,
            FirstName = "",
            LastName = "",
            IdCode = "",
            EMail = new EMail(),
            Role = Role.Customer
        };

        var booking = new Models.Booking
        {
            Id = 1,
            PeriodOfStay = new PeriodOfStay {Start = new DateTime(2024, 7, 5), End = new DateTime(2024, 7, 6)},
            Price = new Money(),
            RoomId = 1,
            UserId = user.Id,
        };
        
        var dateTimeMock = new Mock<IDateTime>();
        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var userRepositoryMock = new Mock<IUserRepository>();

        dateTimeMock.Setup(x => x.Now).Returns(new DateTime(2023, 7, 5));
        bookingRepositoryMock.Setup(x => x.GetBooking(booking.Id)).Returns(booking);
        userRepositoryMock.Setup(x => x.GetUserById(user.Id)).Returns(user);
        
        var bookingService = new BookingService(bookingRepositoryMock.Object, new Mock<IRoomRepository>().Object,
            userRepositoryMock.Object, new Mock<IIdsGenerator>().Object, userContext, dateTimeMock.Object);
        
        bookingService.CancelBooking(booking.Id);

        booking.Status.Should().Be(BookingStatus.Canceled);
    }
    
    [Fact]
    public void CancelBooking_StaffCanCancelCustomersBooking()
    {
        var userContext = new UserContext { UserId = 1 };
        
        var user = new User
        {
            Id = 1,
            FirstName = "",
            LastName = "",
            IdCode = "",
            EMail = new EMail(),
            Role = Role.Staff
        };

        var booking = new Models.Booking
        {
            Id = 1,
            PeriodOfStay = new PeriodOfStay {Start = new DateTime(2024, 7, 5), End = new DateTime(2024, 7, 6)},
            Price = new Money(),
            RoomId = 1,
            UserId = 2
        };

        var dateTimeMock = new Mock<IDateTime>();
        var bookingRepositoryMock = new Mock<IBookingRepository>();
        var userRepositoryMock = new Mock<IUserRepository>();

        dateTimeMock.Setup(x => x.Now).Returns(new DateTime(2023, 7, 5));
        bookingRepositoryMock.Setup(x => x.GetBooking(booking.Id)).Returns(booking);
        userRepositoryMock.Setup(x => x.GetUserById(user.Id)).Returns(user);
        
        var bookingService = new BookingService(bookingRepositoryMock.Object, new Mock<IRoomRepository>().Object,
            userRepositoryMock.Object, new Mock<IIdsGenerator>().Object, userContext, dateTimeMock.Object);
        
        bookingService.CancelBooking(booking.Id);

        booking.Status.Should().Be(BookingStatus.Canceled);
    }
}