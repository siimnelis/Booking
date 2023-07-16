using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Booking.Web.API.Authentication;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (string.IsNullOrEmpty(Request.Path.Value) || Request.Path.Value.StartsWith("/swagger"))
            return Task.FromResult(AuthenticateResult.Fail("anonymous user"));
        
        var authorizationHeader = Request.Headers["Authorization"];
        
        if (!authorizationHeader.Any())
            return Fail();
        
        var stringValue = authorizationHeader[0]!;
        var base64 = stringValue.Substring("Basic ".Length);

        var value = Encoding.UTF8.GetString(Convert.FromBase64String(base64));

        var userId = value.Split(":")[0];

        if (!new List<string>{"1", "2"}.Contains(userId))
            return Fail();
        
        var userIdentity = new UserContextIdentity(int.Parse(userId));
        var userPrincipal = new UserContextIPrincipal(userIdentity);
        
        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(userPrincipal, "Basic")));

    }

    private Task<AuthenticateResult> Fail()
    {
        Response.Headers.Add("WWW-Authenticate","Basic realm=\"\"");
        return Task.FromResult(AuthenticateResult.Fail(""));
    }
    
}