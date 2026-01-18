using Altairis.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Altairis.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Seed(AltairisDbContext context)
    {
        // context.Database.EnsureCreated(); // Asegurar que la DB existe si no se usan migraciones

        if (context.Hotels.Any())
        {
            return;   // La DB ya ha sido sembrada
        }

        var madridHotelId = Guid.NewGuid();
        var bcnHotelId = Guid.NewGuid();
        var parisHotelId = Guid.NewGuid();
        var londonHotelId = Guid.NewGuid();
        var nycHotelId = Guid.NewGuid();
        var tokyoHotelId = Guid.NewGuid();
        var dubaiHotelId = Guid.NewGuid();
        var romeHotelId = Guid.NewGuid();
        var sydneyHotelId = Guid.NewGuid();
        var berlinHotelId = Guid.NewGuid();

        var hotels = new List<Hotel>
        {
            new Hotel { Id = madridHotelId, Name = "Altairis Grand Madrid", Location = "Madrid, España", Rating = 5, Description = "Lujo y confort en el corazón de la capital.", ImageUrl = "https://images.unsplash.com/photo-1566073771259-6a8506099945?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", Status = HotelStatus.Active },
            new Hotel { Id = bcnHotelId, Name = "Altairis Coastal Barcelona", Location = "Barcelona, España", Rating = 4, Description = "Vistas al mar y diseño moderno.", ImageUrl = "https://images.unsplash.com/photo-1520250497591-112f2f40a3f4?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", Status = HotelStatus.Active },
            new Hotel { Id = parisHotelId, Name = "Altairis Eiffel Paris", Location = "París, Francia", Rating = 5, Description = "Elegancia clásica a pasos de la Torre Eiffel.", ImageUrl = "https://images.unsplash.com/photo-1564501049412-61c2a3083791?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", Status = HotelStatus.Active },
            new Hotel { Id = londonHotelId, Name = "Altairis City London", Location = "Londres, Reino Unido", Rating = 4, Description = "Moderno y funcional en el distrito financiero.", ImageUrl = "https://images.unsplash.com/photo-1542314831-068cd1dbfeeb?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", Status = HotelStatus.Active },
            new Hotel { Id = nycHotelId, Name = "Altairis Times Square NYC", Location = "Nueva York, EEUU", Rating = 5, Description = "En el centro de la acción, lujo cosmopolita.", ImageUrl = "https://images.unsplash.com/photo-1571003123894-1f0594d2b5d9?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", Status = HotelStatus.Active },
            
            // Nuevos Hoteles para volumen
            new Hotel { Id = tokyoHotelId, Name = "Altairis Tokyo Shibuya", Location = "Tokio, Japón", Rating = 5, Description = "Tecnología y tradición en armonía.", ImageUrl = "https://images.unsplash.com/photo-1590490360182-c33d57733427?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", Status = HotelStatus.Active },
            new Hotel { Id = dubaiHotelId, Name = "Altairis Palms Dubai", Location = "Dubái, EAU", Rating = 5, Description = "Lujo extremo en la isla artificial.", ImageUrl = "https://images.unsplash.com/photo-1512918760532-3c5a637ddab2?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", Status = HotelStatus.Active },
            new Hotel { Id = romeHotelId, Name = "Altairis Colosseum Rome", Location = "Roma, Italia", Rating = 4, Description = "Historia viva desde tu ventana.", ImageUrl = "https://images.unsplash.com/photo-1551882547-ff40c63fe5fa?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", Status = HotelStatus.Active },
            new Hotel { Id = sydneyHotelId, Name = "Altairis Harbour Sydney", Location = "Sídney, Australia", Rating = 5, Description = "Frente a la Ópera, vistas inigualables.", ImageUrl = "https://images.unsplash.com/photo-1596394516093-501ba68a0ba6?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", Status = HotelStatus.Active },
            new Hotel { Id = berlinHotelId, Name = "Altairis Berlin Mitte", Location = "Berlín, Alemania", Rating = 4, Description = "Arte y cultura en el centro de Europa.", ImageUrl = "https://images.unsplash.com/photo-1445019980597-93fa8acb246c?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80", Status = HotelStatus.Active }
        };

        context.Hotels.AddRange(hotels);
        context.SaveChanges();

        var roomTypes = new List<RoomType>();
        var random = new Random();

        // Generar tipos de habitación para TODOS los hoteles
        foreach (var hotel in hotels)
        {
            var basePrice = 100 + random.Next(0, 400);
            roomTypes.Add(new RoomType { Id = Guid.NewGuid(), HotelId = hotel.Id, Name = "Standard", BasePrice = basePrice, Capacity = 2, Stock = 20 });
            roomTypes.Add(new RoomType { Id = Guid.NewGuid(), HotelId = hotel.Id, Name = "Deluxe View", BasePrice = basePrice * 1.5m, Capacity = 2, Stock = 10 });
            if (hotel.Rating == 5)
            {
                roomTypes.Add(new RoomType { Id = Guid.NewGuid(), HotelId = hotel.Id, Name = "Presidential Suite", BasePrice = basePrice * 4m, Capacity = 4, Stock = 2 });
            }
        }

        context.RoomTypes.AddRange(roomTypes);
        context.SaveChanges();

        // Generar Inventario (Próximos 60 días)
        var inventory = new List<Inventory>();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        foreach (var rt in roomTypes)
        {
            for (int i = 0; i < 60; i++)
            {
                var date = today.AddDays(i);
                var isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
                var priceMod = isWeekend ? 1.2m : 1.0m;
                
                inventory.Add(new Inventory
                {
                    Id = Guid.NewGuid(),
                    RoomTypeId = rt.Id,
                    Date = date,
                    Available = rt.Stock - random.Next(0, 3), // Ocupación aleatoria ligera
                    Price = rt.BasePrice * priceMod
                });
            }
        }
        context.Inventories.AddRange(inventory);

        // Generar Reservas masivas (Pasadas y Futuras para gráficos)
        var bookings = new List<Booking>();
        var guests = new[] { "Juan Pérez", "Maria Garcia", "John Smith", "Emma Wilson", "Hans Müller", "Yuki Tanaka", "Ahmed Al-Fayed", "Sofia Rossi", "Liam O'Connor", "Chloe Dubois" };
        
        // 50 Reservas pasadas (últimos 30 días)
        for(int i = 0; i < 50; i++)
        {
            var rt = roomTypes[random.Next(roomTypes.Count)];
            var daysAgo = random.Next(1, 30);
            var duration = random.Next(1, 5);
            var checkIn = today.AddDays(-daysAgo);
            
            bookings.Add(new Booking
            {
                Id = Guid.NewGuid(),
                HotelId = rt.HotelId,
                RoomTypeId = rt.Id,
                GuestName = guests[random.Next(guests.Length)],
                CheckIn = checkIn,
                CheckOut = checkIn.AddDays(duration),
                TotalPrice = rt.BasePrice * duration,
                Status = BookingStatus.Confirmed,
                CreatedAt = DateTime.UtcNow.AddDays(-daysAgo)
            });
        }

        // 20 Reservas futuras
        for(int i = 0; i < 20; i++)
        {
            var rt = roomTypes[random.Next(roomTypes.Count)];
            var daysAhead = random.Next(1, 30);
            var duration = random.Next(1, 7);
            var checkIn = today.AddDays(daysAhead);
            
            bookings.Add(new Booking
            {
                Id = Guid.NewGuid(),
                HotelId = rt.HotelId,
                RoomTypeId = rt.Id,
                GuestName = guests[random.Next(guests.Length)],
                CheckIn = checkIn,
                CheckOut = checkIn.AddDays(duration),
                TotalPrice = rt.BasePrice * duration,
                Status = BookingStatus.Confirmed,
                CreatedAt = DateTime.UtcNow
            });
        }

        context.Bookings.AddRange(bookings);
        context.SaveChanges();
    }
}
