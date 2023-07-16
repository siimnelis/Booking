using Booking.Exceptions;
using Booking.ValueTypes;
using FluentAssertions;
using Moq;
using Xunit;

namespace Booking.UnitTests;

public class PeriodOfStayTests
{
    [Fact]
    public void Constructor_PeriodCantBeInThePast()
    {
        var dateTimeMoq = new Mock<IDateTime>();
        dateTimeMoq.Setup(x => x.Now).Returns(new DateTime(2023, 7, 5));

        var act = () =>
        {
            new PeriodOfStay(new DateTime(2023, 7, 4), new DateTime(2023, 7, 5), dateTimeMoq.Object);
        };

        act.Should().Throw<PeriodOfStayCantBeInThePastException>();
    }
    
    [Fact]
    public void Constructor_EndCantBeSmallerThanStart()
    {
        var dateTimeMoq = new Mock<IDateTime>();
        dateTimeMoq.Setup(x => x.Now).Returns(new DateTime(2023, 7, 5));
        
        var act = () =>
        {
            new PeriodOfStay(new DateTime(2023, 7, 5), new DateTime(2023, 7, 4), dateTimeMoq.Object);
        };

        act.Should().Throw<PeriodOfStayMustBeAtleastForADayException>();
    }
    
    [Fact]
    public void Constructor_EndCantBeOnTheSameDayAsStart()
    {
        var dateTimeMoq = new Mock<IDateTime>();
        dateTimeMoq.Setup(x => x.Now).Returns(new DateTime(2023, 7, 5));
        
        var act = () =>
        {
            new PeriodOfStay(new DateTime(2023, 7, 5), new DateTime(2023, 7, 5), dateTimeMoq.Object);
        };

        act.Should().Throw<PeriodOfStayMustBeAtleastForADayException>();
    }

    [Fact]
    public void NumberOfDays_CalculatesRightNumbersOfDaysStayed()
    {
        var dateTimeMoq = new Mock<IDateTime>();
        dateTimeMoq.Setup(x => x.Now).Returns(new DateTime(2023, 7, 5));

        var periodOfStay = new PeriodOfStay(new DateTime(2023, 7, 5), new DateTime(2023, 7, 7), dateTimeMoq.Object);

        var numberOfDays = periodOfStay.NumberOfDays;

        numberOfDays.Should().Be(2);
    }
    
    [Theory]
    [MemberData(nameof(OverlapsWith_PeriodsOverlap_Data))]
    public void OverlapsWith_PeriodsOverlap(PeriodOfStay first, PeriodOfStay second)
    {
        var result = first.OverlapsWith(second);

        result.Should().Be(true);
    }

    [Theory]
    [MemberData(nameof(OverlapsWith_PeriodsDoesntOverlap_Data))]
    public void OverlapsWith_PeriodsDoesntOverlap(PeriodOfStay first, PeriodOfStay second)
    {
        var result = first.OverlapsWith(second);

        result.Should().Be(false);
    }
    
    public static IEnumerable<object[]> OverlapsWith_PeriodsOverlap_Data()
    {
        return new List<object[]>
        {
            new object[]
            {
                new PeriodOfStay{Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 9)},
                new PeriodOfStay {Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 7)}
            },
            new object[]
            {
                new PeriodOfStay{Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 9)},
                new PeriodOfStay{Start = new DateTime(2023, 7, 6), End = new DateTime(2023, 7, 7)}
            },
            new object[]
            {
                new PeriodOfStay{Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 9)},
                new PeriodOfStay{Start = new DateTime(2023, 7, 7), End = new DateTime(2023, 7, 11)}
            },
            new object[]
            {
                new PeriodOfStay{Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 9)},
                new PeriodOfStay{Start = new DateTime(2023, 7, 6), End = new DateTime(2023, 7, 7)}
            },
            new object[]
            {
                new PeriodOfStay{Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 9)},
                new PeriodOfStay{Start = new DateTime(2023, 6, 30), End = new DateTime(2023, 7, 10)}
            },
            new object[]
            {
                new PeriodOfStay{Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 9)}, 
                new PeriodOfStay{Start = new DateTime(2023, 6, 30), End = new DateTime(2023, 7, 6)},
            },
            new object[]
            {
                new PeriodOfStay{Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 9)}, 
                new PeriodOfStay{Start = new DateTime(2023, 6, 30), End = new DateTime(2023, 7, 9)}
            },
            new object[]
            {
                new PeriodOfStay{Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 9)}, 
                new PeriodOfStay{Start = new DateTime(2023, 6, 30), End = new DateTime(2024, 7, 9)}
            }
        };
    }
    
    public static IEnumerable<object[]> OverlapsWith_PeriodsDoesntOverlap_Data()
    {
        return new List<object[]>
        {
            new object[]
            {
                new PeriodOfStay{Start = new DateTime(2023, 7, 6), End = new DateTime(2023, 7, 5)},
                new PeriodOfStay{Start = new DateTime(2023, 7, 6), End = new DateTime(2023, 7, 6)}
            },
            new object[]
            {
                new PeriodOfStay{Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 6)},
                new PeriodOfStay{Start = new DateTime(2023, 6, 30), End = new DateTime(2023, 7, 5)}
            },
            new object[]
            {
                new PeriodOfStay{Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 6)},
                new PeriodOfStay{Start = new DateTime(2023, 6, 30), End = new DateTime(2023, 7, 4)}
            },
            new object[]
            {
                new PeriodOfStay{Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 6)},
                new PeriodOfStay{Start = new DateTime(2023, 7, 7), End = new DateTime(2023, 8, 12)}
            },
            new object[]
            {
                new PeriodOfStay{Start = new DateTime(2023, 7, 5), End = new DateTime(2023, 7, 6)},
                new PeriodOfStay{Start = new DateTime(2024, 7, 7), End = new DateTime(2024, 8, 12)}
            }
        };
    }
}