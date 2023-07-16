using Booking.ValueTypes;

namespace Booking.Models;

public class User
{
    internal User()
    {
        
    }

    public User(int id, string firstName, string lastName, string idCode, EMail eMail, Role role)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        IdCode = idCode;
        EMail = eMail;
        Role = role;
    }
    
    public int Id { get; internal set; }
    public string FirstName { get; internal set; }
    public string LastName { get; internal set; }
    public string IdCode { get; internal set; }
    public EMail EMail { get; internal set; }
    public Role Role { get; internal set; }
}