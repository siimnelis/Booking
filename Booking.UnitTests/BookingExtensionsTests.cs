using Booking.Exceptions;
using Booking.Extensions;
using Booking.ValueTypes;
using FluentAssertions;
using Moq;
using Xunit;

namespace Booking.UnitTests;

public class BookingExtensionsTests
{
    [Fact]
    public void AnyOverlapingBookingsForARoom_HasOverlapingBookings()
    {
        var dateTimeMoq = new Mock<IDateTime>();
        var periodOfStay = new PeriodOfStay(new DateTime(2023, 7, 5), new DateTime(2023, 7, 6),
            dateTimeMoq.Object);
        
        var bookings = new List<Models.Booking>()
        {
            new ()
            {
                Id = 1,
                PeriodOfStay = new PeriodOfStay{Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 6)},
                Price = new Money(),
                RoomId = 1,
                UserId = 1
            }
        };

        var result = bookings.AnyOverlapingBookingsForARoom(periodOfStay);

        result.Should().Be(true);
    }

    [Fact]
    public void AnyOverlapingBookingsForARoom_HasNoOverlapingBookings()
    {
        var periodOfStay = new PeriodOfStay{Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 6)};
        
        var bookings = new List<Models.Booking>()
        {
            new ()
            {
                Id = 1,
                PeriodOfStay = new PeriodOfStay{Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 6)},
                Price = new Money(),
                RoomId = 1,
                UserId = 1,
                Status = BookingStatus.Canceled
            },
            new ()
            {
                Id = 1,
                PeriodOfStay = new PeriodOfStay{Start = new DateTime(2024, 7, 5), End = new DateTime(2024, 7, 6)},
                    Price = new Money(),
                RoomId = 1,
                UserId = 1,
            }
        };
        
        var result = bookings.AnyOverlapingBookingsForARoom(periodOfStay);

        result.Should().Be(false);
    }
}