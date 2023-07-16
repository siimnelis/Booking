using System.Security.Claims;

namespace Booking.Web.API.Authentication;

public class UserContextIdentity : ClaimsIdentity
{
    public int UserId { get; }
    public UserContextIdentity(int userId):base("Basic")
    {
        UserId = userId;
    }
}