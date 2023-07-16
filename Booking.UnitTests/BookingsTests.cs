using Booking.Exceptions;
using Booking.ValueTypes;
using FluentAssertions;
using Moq;
using Xunit;

namespace Booking.UnitTests;

public class BookingsTests
{
    [Fact]
    public void Cancel_CantCancel3DaysBeforePerioOfStay()
    {
        var dateTimeMoq = new Mock<IDateTime>();
        dateTimeMoq.Setup(x => x.Now).Returns(new DateTime(2023, 7, 2));
        
        var booking = new Models.Booking
        {
            Id = 1,
            PeriodOfStay = new PeriodOfStay
            {
                Start = new DateTime(2023, 7, 5),
                End = new DateTime(2023,7,6)
            },
            Price = new Money(),
            RoomId = 1,
            UserId = 1
        };

        var act = () =>
        {
            booking.Cancel(dateTimeMoq.Object);
        };

        act.Should().Throw<CantCancelBooking3DaysBeforePeriodOfStayException>();
    }
    
    [Fact]
    public void Cancel_CanCancelBooking()
    {
        var dateTimeMoq = new Mock<IDateTime>();
        dateTimeMoq.Setup(x => x.Now).Returns(new DateTime(2023, 7, 1));
        
        var booking = new Models.Booking
        {
            Id = 1,
            PeriodOfStay = new PeriodOfStay
            {
                Start = new DateTime(2023, 7, 5),
                End = new DateTime(2023,7,6)
            },
            Price = new Money(),
            RoomId = 1,
            UserId = 1
        };

        booking.Cancel(dateTimeMoq.Object);

        booking.Status.Should().Be(BookingStatus.Canceled);
    }
}