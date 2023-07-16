using Booking.ValueTypes;

namespace Booking.Models;

public class Room
{
    internal Room()
    {
        
    }
    
    public Room(int id, int number, int numberOfBedPlaces, Money price)
    {
        Id = id;
        Number = number;
        NumberOfBedPlaces = numberOfBedPlaces;
        Price = price;
    }
    
    public int Id { get; internal set; }
    public int Number { get; internal set; }
    public int NumberOfBedPlaces { get; internal set; }
    public Money Price { get; internal set; }
}