using Booking.Exceptions;
using Booking.Models;
using Booking.ValueTypes;
using FluentAssertions;
using Xunit;

namespace Booking.Repositories.EntityFramework.UnitTests;

public class BookingRepositoryTests
{
    [Fact]
    public void Add_CanAddBooking()
    {
        var bookingContext = new BookingContext();
        var addedBooking = new Models.Booking(1, 1, 1, new Money(10), new PeriodOfStay{Start = new DateTime(2023,7,5), End = new DateTime(2023,7,6)});
        
        var bookingRepository = new BookingRepository(bookingContext);

        bookingRepository.Add(addedBooking);

        var booking = bookingContext.Bookings.Local.FirstOrDefault(x => x.Id == addedBooking.Id);

        booking.Should().NotBeNull();
        booking.Should().BeSameAs(addedBooking);
    }

    [Fact]
    public void GetBooking_ThrowsExceptionWhenBookingNotFound()
    {
        var bookingContext = new BookingContext();
        var bookingRepository = new BookingRepository(bookingContext);
        
        var act = () =>
        {
            bookingRepository.GetBooking(1);
        };

        act.Should().Throw<NoBookingFoundException>();
    }

    [Fact]
    public void GetBooking_ReturnsUserBooking()
    {
        var bookingContext = new BookingContext();

        var expectedBooking = new Models.Booking {Id = 1, UserId = 1};

        bookingContext.Bookings.Add(expectedBooking);
        bookingContext.Bookings.Add(new Models.Booking {Id = 2, UserId = 1});
        bookingContext.Bookings.Add(new Models.Booking {Id = 3, UserId = 2});
        
        var bookingRepository = new BookingRepository(bookingContext);

        var booking = bookingRepository.GetBooking(1);

        booking.Should().NotBeNull();
        booking.Should().BeSameAs(expectedBooking);
    }
    
    [Fact]
    public void GetUserBooking_ReturnsUsersBookings()
    {
        var bookingContext = new BookingContext();

        var expectedBookings = new List<Models.Booking>()
        {
            new()
            {
                Id = 1, UserId = 1
            },
            new()
            {
                Id = 2, UserId = 1
            }
        };

        foreach (var expectedBooking in expectedBookings)
            bookingContext.Bookings.Add(expectedBooking);

        bookingContext.Bookings.Add(new Models.Booking {Id = 3, UserId = 3});
        bookingContext.Bookings.Add(new Models.Booking {Id = 4, UserId = 2});
        
        var bookingRepository = new BookingRepository(bookingContext);

        var bookings = bookingRepository.GetUserBookings(new User{Id = 1});

        bookings.Should().NotBeNull();
        bookings.Should().BeEquivalentTo(expectedBookings);
    }

    [Fact]
    public void GetBookingsForARoom_ReturnsBookingsForARoom()
    {
        var bookingContext = new BookingContext();

        var expectedBookings = new List<Models.Booking>()
        {
            new()
            {
                Id = 1, UserId = 1,
                RoomId = 1
            },
            new()
            {
                Id = 2, UserId = 2,
                RoomId = 1
            }
        };

        bookingContext.Rooms.Add(new Room {Id = 1});
        bookingContext.Rooms.Add(new Room {Id = 2});

        bookingContext.Bookings.Add(new Models.Booking
        {
            Id = 3, RoomId = 2
        });
        
        foreach (var expectedBooking in expectedBookings)
            bookingContext.Bookings.Add(expectedBooking);
        
        var bookingRepository = new BookingRepository(bookingContext);

        var bookings = bookingRepository.GetBookingsForARoom(new Room {Id = 1});

        bookings.Should().NotBeNull();
        bookings.Should().BeEquivalentTo(expectedBookings);
    }
}