using Altairis.API.DTOs;
using Altairis.Domain.Entities;
using Altairis.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Altairis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly AltairisDbContext _context;

    public InventoryController(AltairisDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryResponse>>> GetInventory([FromQuery] Guid hotelId, [FromQuery] DateOnly start, [FromQuery] DateOnly end)
    {
        var inventories = await _context.Inventories
            .Include(i => i.RoomType)
            .Where(i => i.RoomType!.HotelId == hotelId && i.Date >= start && i.Date <= end)
            .AsNoTracking()
            .ToListAsync();

        return Ok(inventories.Select(i => new InventoryResponse(i.RoomTypeId, i.Date, i.Available, i.Price)));
    }

    [HttpPost("batch")]
    public async Task<ActionResult> BatchUpdate(BatchInventoryRequest request)
    {
        // Obtener todos los tipos de habitación para este hotel
        var roomTypes = await _context.RoomTypes
            .Where(rt => rt.HotelId == request.HotelId)
            .Select(rt => new { rt.Id, rt.BasePrice })
            .ToListAsync();

        var days = request.EndDate.DayNumber - request.StartDate.DayNumber + 1;
        if (days <= 0) return BadRequest("Invalid date range");

        // Estrategia simple: Lógica de Upsert (Borrar existentes en rango e insertar nuevos)
        // Para demo MVP, esto asegura un estado limpio para el rango. En prod, usar MERGE/Upsert.
        
        var existing = await _context.Inventories
            .Where(i => i.RoomType!.HotelId == request.HotelId && i.Date >= request.StartDate && i.Date <= request.EndDate)
            .ToListAsync();
        
        _context.Inventories.RemoveRange(existing);

        var newEntries = new List<Inventory>();
        for (int i = 0; i < days; i++)
        {
            var date = request.StartDate.AddDays(i);
            foreach (var rt in roomTypes)
            {
                newEntries.Add(new Inventory
                {
                    Id = Guid.NewGuid(),
                    RoomTypeId = rt.Id,
                    Date = date,
                    Available = 10, // Disponibilidad por defecto, idealmente vendría de request o Stock
                    Price = request.PriceModifier ?? rt.BasePrice // Aplicar modificador o base
                });
            }
        }

        _context.Inventories.AddRange(newEntries);
        await _context.SaveChangesAsync();

        return Ok(new { Message = $"Updated {newEntries.Count} inventory records." });
    }
    [HttpPut]
    public async Task<ActionResult> UpdateInventory(UpdateInventoryRequest request)
    {
        var inventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.RoomTypeId == request.RoomTypeId && i.Date == request.Date);

        if (inventory == null)
        {
            // Crear si no existe
            inventory = new Inventory
            {
                Id = Guid.NewGuid(),
                RoomTypeId = request.RoomTypeId,
                Date = request.Date,
                Available = request.Available,
                Price = request.Price
            };
            _context.Inventories.Add(inventory);
        }
        else
        {
            inventory.Available = request.Available;
            inventory.Price = request.Price;
            _context.Entry(inventory).State = EntityState.Modified;
        }

        await _context.SaveChangesAsync();
        return Ok(new { Message = "Inventory updated" });
    }
}
