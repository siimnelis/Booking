using Booking.Models;

namespace Booking.Repositories;

public interface IUserRepository
{
    User GetUserById(int userId);
    void Add(User user);
}