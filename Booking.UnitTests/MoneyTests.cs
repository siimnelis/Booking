using Booking.ValueTypes;
using FluentAssertions;
using Xunit;

namespace Booking.UnitTests;

public class MoneyTests
{
    [Fact]
    public void Constructor_CannotCreateMoneyWithNegativeValue()
    {
        var act = () =>
        {
            new Money(-1.12m);
        };
 
        act.Should().Throw<ArgumentException>().WithMessage("Money can't be a negative amount.");
    }
}