using Altairis.API.DTOs;
using Altairis.Domain.Entities;
using Altairis.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Altairis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomTypesController : ControllerBase
{
    private readonly AltairisDbContext _context;

    public RoomTypesController(AltairisDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomTypeResponse>>> GetRoomTypes([FromQuery] Guid hotelId)
    {
        var roomTypes = await _context.RoomTypes
            .Where(rt => rt.HotelId == hotelId)
            .Select(rt => new RoomTypeResponse(rt.Id, rt.HotelId, rt.Name, rt.BasePrice, rt.Capacity, rt.Stock))
            .ToListAsync();

        return Ok(roomTypes);
    }


    [HttpPost]
    public async Task<ActionResult<RoomTypeResponse>> CreateRoomType(CreateRoomTypeRequest request)
    {
        var roomType = new RoomType
        {
            Id = Guid.NewGuid(),
            HotelId = request.HotelId,
            Name = request.Name,
            BasePrice = request.BasePrice,
            Capacity = request.Capacity,
            Stock = request.Stock
        };

        _context.RoomTypes.Add(roomType);
        await _context.SaveChangesAsync();

        return Ok(new RoomTypeResponse(roomType.Id, roomType.HotelId, roomType.Name, roomType.BasePrice, roomType.Capacity, roomType.Stock));
    }
}
