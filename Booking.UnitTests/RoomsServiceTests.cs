using Booking.Models;
using Booking.Repositories;
using Booking.ValueTypes;
using FluentAssertions;
using Moq;
using Xunit;

namespace Booking.UnitTests;

public class RoomsServiceTests
{
    [Fact]
    public void GetRoomsAvailableForPeriodOfStay_ReturnsRooms()
    {
        var periodOfStay = new PeriodOfStay{Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 6)};
        
        var roomRepositoryMock = new Mock<IRoomRepository>();

        var expectedRooms = new List<Room>()
        {
            new()
            {
                Id = 1,
                Price = new Money(),
                NumberOfBedPlaces = 1,
                Number = 1
            }
        };
        
        roomRepositoryMock.Setup(x => x.GetRoomsAvailableForPeriodOfStay(periodOfStay)).Returns(expectedRooms);

        var bookingService = new RoomsService(roomRepositoryMock.Object);

        var rooms = bookingService.GetRoomsAvailableForPeriodOfStay(periodOfStay);

        roomRepositoryMock.Verify(x => x.GetRoomsAvailableForPeriodOfStay(periodOfStay), Times.Once);
        rooms.Should().BeEquivalentTo(expectedRooms);
    }
}