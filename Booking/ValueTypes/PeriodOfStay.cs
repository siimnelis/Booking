using Booking.Exceptions;

namespace Booking.ValueTypes;

public record PeriodOfStay
{
    internal PeriodOfStay()
    {
        
    }
    
    public PeriodOfStay(DateTime start, DateTime end, IDateTime dateTime)
    {
        Start = start.Date;
        End = end.Date;

        if (Start < dateTime.Now.Date)
            throw new PeriodOfStayCantBeInThePastException();
        
        if (End.AddDays(-1) < Start)
            throw new PeriodOfStayMustBeAtleastForADayException();
        
    }
    
    public DateTime Start { get; internal set; }
    public DateTime End { get; internal set; }

    public bool OverlapsWith(PeriodOfStay periodOfStay)
    {
        return Start <= periodOfStay.Start && End > periodOfStay.Start
            || Start >= periodOfStay.Start && Start < periodOfStay.End;
    }

    public int NumberOfDays => (int) (End - Start).TotalDays;
}