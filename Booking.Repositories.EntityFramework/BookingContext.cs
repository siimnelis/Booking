using Booking.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking.Repositories.EntityFramework;

public class BookingContext : DbContext
{
    public DbSet<Models.Booking> Bookings { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("Bookings");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var bookingBuilder = modelBuilder.Entity<Models.Booking>();
        
        bookingBuilder.HasKey(b => b.Id);
        bookingBuilder.Property(b => b.RoomId).IsRequired();
        bookingBuilder.Property(b => b.UserId).IsRequired();
        bookingBuilder.OwnsOne(x => x.Price).Property(x=>x.Value).HasColumnName("Price");
        bookingBuilder.Property(x => x.Status).IsRequired();
        
        var periodOfStayBuilder = bookingBuilder.OwnsOne(x => x.PeriodOfStay);
        periodOfStayBuilder.Property(x => x.Start).HasColumnName("PeriodOfStayStart");
        periodOfStayBuilder.Property(x => x.End).HasColumnName("PeriodOfStayEnd");



        var roomBuilder = modelBuilder.Entity<Room>();

        roomBuilder.HasKey(x => x.Id);
        roomBuilder.Property(x => x.Number).IsRequired();
        roomBuilder.Property(x => x.NumberOfBedPlaces).IsRequired();
        roomBuilder.OwnsOne(x => x.Price).Property(x=>x.Value).HasColumnName("Price");

        var userBuilder = modelBuilder.Entity<User>();

        userBuilder.HasKey(x => x.Id);
        userBuilder.Property(x => x.Role).IsRequired();
        userBuilder.Property(x => x.FirstName).IsRequired();
        userBuilder.Property(x => x.LastName).IsRequired();
        userBuilder.Property(x => x.IdCode).IsRequired();
        userBuilder.OwnsOne(x => x.EMail).Property(x => x.Value).HasColumnName("EMail");
    }
}