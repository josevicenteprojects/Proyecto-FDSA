using Altairis.API.DTOs;
using Altairis.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Altairis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatsController : ControllerBase
{
    private readonly AltairisDbContext _context;

    public StatsController(AltairisDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<DashboardStatsResponse>> GetStats()
    {
        var activeHotels = await _context.Hotels.CountAsync();
        var totalBookings = await _context.Bookings.CountAsync();
        
        // Cálculo simple para huéspedes (suma o recuento). Asumiendo 2 huéspedes por reserva para aproximación MVP si falta el campo
        // o simplemente contar reservas como "activas" por ahora.
        // Vamos a confiar en el recuento de GuestName de Booking por ahora.
        var totalGuests = await _context.Bookings.Select(b => b.GuestName).Distinct().CountAsync();

        // Tasa de Ocupación: (Habitaciones Reservadas / Total Habitaciones) * 100
        // MVP: Solo simular cálculo estricto o usar un marcador de posición si es 0
        var totalStock = await _context.RoomTypes.SumAsync(rt => rt.Stock);
        var occupancy = totalStock > 0 ? (double)totalBookings / totalStock * 100 : 0;

        return Ok(new DashboardStatsResponse(
            ActiveHotels: activeHotels,
            TotalBookings: totalBookings,
            TotalGuests: totalGuests,
            OccupancyRate: Math.Round(occupancy, 1)
        ));
    }
}
