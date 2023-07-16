using Booking.Exceptions;
using Booking.Models;
using FluentAssertions;
using Xunit;

namespace Booking.Repositories.EntityFramework.UnitTests;

public class UserRepositoryTests
{
    [Fact]
    public void GetUserById_ThrowsExceptionWhenUserNotFound()
    {
        var userRepository = new UserRepository(new BookingContext());

        var act = () =>
        {
            userRepository.GetUserById(1);
        };

        act.Should().Throw<UserNotFoundException>();
    }
    
    [Fact]
    public void GetUserById_ReturnsUser()
    {
        var bookingContext = new BookingContext();

        var expectedUser = new User {Id = 1};

        bookingContext.Users.Add(expectedUser);
        bookingContext.Users.Add(new User {Id = 2});
        
        var userRepository = new UserRepository(bookingContext);

        var user = userRepository.GetUserById(1);

        user.Should().NotBeNull();
        user.Should().BeSameAs(expectedUser);
    }

    [Fact]
    public void Add_CanAddUser()
    {
        var bookingContext = new BookingContext();

        var addedUser = new User {Id = 1};
        
        bookingContext.Users.Add(addedUser);
        
        var userRepository = new UserRepository(bookingContext);
        
        userRepository.Add(addedUser);

        var user = userRepository.GetUserById(1);

        user.Should().BeSameAs(addedUser);
    }
}