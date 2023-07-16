using System.Text.RegularExpressions;
using Booking.Exceptions;

namespace Booking.ValueTypes;

public record EMail
{
    public string Value { get; internal set; }

    internal EMail()
    {
        
    }
    
    public EMail(string value)
    {
        if (!IsValid(value))
            throw new EMailIsInAInvalidFormatException();
        
        Value = value;
    }
    
    public static bool IsValid(string value)
    {
        return Regex.IsMatch(value,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
    }
}