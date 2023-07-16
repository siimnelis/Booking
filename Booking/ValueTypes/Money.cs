namespace Booking.ValueTypes;

public record Money
{
    public decimal Value { get; internal set; }

    internal Money()
    {
        
    }

    public Money(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Money can't be a negative amount.");

        Value = value;
    }
}