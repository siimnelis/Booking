using Booking.Models;
using Booking.Repositories;
using Booking.ValueTypes;

namespace Booking.Web.API.Extensions;

public static class WebApplicationExtensions
{
    public static void Init(this WebApplication webApplication)
    {
        InitData(webApplication.Services);
    }
    
    private static void InitData(IServiceProvider serviceProvider)
    {
        var idsGenerator = serviceProvider.GetService<IIdsGenerator>()!;
        
        AddRooms(serviceProvider.GetService<IRoomRepository>()!, idsGenerator);
        AddUsers(serviceProvider.GetService<IUserRepository>()!, idsGenerator);
    }

    private static void AddRooms(IRoomRepository roomRepository, IIdsGenerator idsGenerator)
    {
        roomRepository.Add(new Room(idsGenerator.GenerateRoomId(), 100, 1,new Money(65)));
        roomRepository.Add(new Room(idsGenerator.GenerateRoomId(), 101, 1,new Money(75)));
        roomRepository.Add(new Room(idsGenerator.GenerateRoomId(), 200, 2,new Money(85)));
        roomRepository.Add(new Room(idsGenerator.GenerateRoomId(), 201, 2,new Money(85)));
        roomRepository.Add(new Room(idsGenerator.GenerateRoomId(), 300, 3,new Money(105)));
        roomRepository.Add(new Room(idsGenerator.GenerateRoomId(), 301, 3,new Money(125)));
    }

    private static void AddUsers(IUserRepository userRepository, IIdsGenerator idsGenerator)
    {
        userRepository.Add(new User(idsGenerator.GenerateUserId(), "John", "Doe", "39311220023", new EMail("john.doe@booking.com"), Role.Staff));
        userRepository.Add(new User(idsGenerator.GenerateUserId(), "Mary", "Musk", "49311220023", new EMail("mary.musk@gmail.com"), Role.Customer));
        userRepository.Add(new User(idsGenerator.GenerateUserId(), "Peter", "Dusk", "39211220023", new EMail("peter.dusk@gmail.com"), Role.Customer));
    }
}