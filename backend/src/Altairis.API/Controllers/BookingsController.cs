using Altairis.API.DTOs;
using Altairis.Domain.Entities;
using Altairis.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Altairis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly AltairisDbContext _context;

    public BookingsController(AltairisDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingResponse>>> GetBookings([FromQuery] string filter = "all", [FromQuery] string sortOrder = "desc")
    {
        // Unir propiedades de navegación
        var query = _context.Bookings
            .Join(_context.RoomTypes, b => b.RoomTypeId, rt => rt.Id, (b, rt) => new { b, rt })
            .Join(_context.Hotels, combined => combined.rt.HotelId, h => h.Id, (combined, h) => new { combined.b, combined.rt, h })
            .AsQueryable();

        // Aplicar filtros
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        switch (filter.ToLower())
        {
            case "upcoming":
                query = query.Where(x => x.b.CheckIn >= today && x.b.Status != BookingStatus.Cancelled);
                break;
            case "past":
                query = query.Where(x => x.b.CheckOut < today && x.b.Status != BookingStatus.Cancelled);
                break;
            case "cancelled":
                query = query.Where(x => x.b.Status == BookingStatus.Cancelled);
                break;
            case "active":
                query = query.Where(x => x.b.CheckIn <= today && x.b.CheckOut >= today && x.b.Status == BookingStatus.CheckedIn);
                break;
            default: 
                break;
        }

        // Aplicar Ordenamiento
        if (sortOrder.ToLower() == "asc")
        {
            query = query.OrderBy(x => x.b.CheckIn);
        }
        else
        {
            query = query.OrderByDescending(x => x.b.CheckIn);
        }

        var bookings = await query
            .Select(x => new BookingResponse(
                x.b.Id.ToString(),
                x.b.GuestName,
                x.h.Name,
                x.rt.Name,
                x.b.CheckIn,
                x.b.CheckOut,
                x.b.Status.ToString(),
                x.b.TotalPrice
            ))
            .ToListAsync();

        return Ok(bookings);
    }

    [HttpPost]
    public async Task<ActionResult<BookingResponse>> CreateBooking(CreateBookingRequest request)
    {
        var roomType = await _context.RoomTypes.FindAsync(request.RoomTypeId);
        if (roomType == null) return BadRequest("Invalid Room Type");

        var nights = request.CheckOut.DayNumber - request.CheckIn.DayNumber;
        var total = nights * roomType.BasePrice; // Precio simplificado

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            HotelId = request.HotelId,
            RoomTypeId = request.RoomTypeId,
            GuestName = request.GuestName,
            CheckIn = request.CheckIn,
            CheckOut = request.CheckOut,
            Status = BookingStatus.Confirmed,
            TotalPrice = total,
            CreatedAt = DateTime.UtcNow
        };

        _context.Bookings.Add(booking);
        // La lógica para disminuir inventario iría aquí
        await _context.SaveChangesAsync();

        return Ok(new { BookingId = booking.Id, Status = "Confirmed" });
    }
}
