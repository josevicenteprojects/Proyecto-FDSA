using Altairis.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Altairis.Infrastructure.Persistence;

public class AltairisDbContext : DbContext
{
    public AltairisDbContext(DbContextOptions<AltairisDbContext> options) : base(options)
    {
    }

    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de Hotel
        modelBuilder.Entity<Hotel>()
            .Property(h => h.Name).IsRequired().HasMaxLength(100);
        
        // Configuración de Tipo de Habitación
        modelBuilder.Entity<RoomType>()
            .Property(rt => rt.BasePrice).HasPrecision(18, 2);

        modelBuilder.Entity<RoomType>()
            .HasOne(rt => rt.Hotel)
            .WithMany(h => h.RoomTypes)
            .HasForeignKey(rt => rt.HotelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración de Inventario
        modelBuilder.Entity<Inventory>()
            .Property(i => i.Price).HasPrecision(18, 2);

        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.RoomType)
            .WithMany(rt => rt.Inventories)
            .HasForeignKey(i => i.RoomTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración de Reserva
        modelBuilder.Entity<Booking>()
            .Property(b => b.TotalPrice).HasPrecision(18, 2);
    }
}
