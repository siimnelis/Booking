using Booking.Web.API.Authentication;

namespace Booking.Web.API.Extensions;

public static class UserContextExtensions
{
    public static void AddUserContext(this IServiceCollection services)
    {
        services.AddScoped<UserContext>(serviceProvider =>
        {
            var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>()!;
            var httpContext = httpContextAccessor.HttpContext!;
    
            if (httpContext.User.Identity is UserContextIdentity {IsAuthenticated: true} userContextIdentity)
            {
                return new UserContext
                {
                    UserId = userContextIdentity.UserId
                };
            }

            return new UserContext
            {
                UserId = 0
            };
        });
    }
}