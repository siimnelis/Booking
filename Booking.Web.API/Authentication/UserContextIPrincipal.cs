using System.Security.Claims;

namespace Booking.Web.API.Authentication;

public class UserContextIPrincipal : ClaimsPrincipal
{
    public UserContextIPrincipal(UserContextIdentity userContextIdentity) : base(userContextIdentity)
    {

    }
}