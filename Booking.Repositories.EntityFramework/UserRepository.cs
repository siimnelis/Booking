using Booking.Exceptions;
using Booking.Models;

namespace Booking.Repositories.EntityFramework;

public class UserRepository : IUserRepository
{
    private BookingContext BookingContext { get; }

    public UserRepository(BookingContext bookingContext)
    {
        BookingContext = bookingContext;
    }
    public User GetUserById(int userId)
    {
        var user = BookingContext.Users.Local.FirstOrDefault(x => x.Id == userId);

        if (user == null)
            throw new UserNotFoundException();
        
        return user;
    }
    
    public void Add(User user)
    {
        BookingContext.Users.Add(user);
    }
}