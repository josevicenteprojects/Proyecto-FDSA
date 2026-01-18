using Altairis.API.DTOs;
using Altairis.Domain.Entities;
using Altairis.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Altairis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly AltairisDbContext _context;

    public HotelsController(AltairisDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<HotelResponse>>> GetHotels([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = _context.Hotels.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(h => h.Name.Contains(search) || h.Location.Contains(search));
        }

        var totalCount = await query.CountAsync();

        var hotels = await query
            .OrderByDescending(h => h.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(h => new HotelResponse(h.Id, h.Name, h.Location, h.Rating, h.Status.ToString(), h.RoomTypes.Count, h.ImageUrl))
            .ToListAsync();

        return Ok(new PagedResponse<HotelResponse>(hotels, totalCount, page, pageSize));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HotelDetailResponse>> GetHotel(Guid id)
    {
        var hotel = await _context.Hotels
            .Include(h => h.RoomTypes)
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hotel == null) return NotFound();

        var roomTypes = hotel.RoomTypes
            .Select(rt => new RoomTypeResponse(rt.Id, rt.HotelId, rt.Name, rt.BasePrice, rt.Capacity, rt.Stock));

        return Ok(new HotelDetailResponse(hotel.Id, hotel.Name, hotel.Location, hotel.Rating, hotel.Status.ToString(), hotel.Description ?? "", hotel.ImageUrl, roomTypes));
    }

    [HttpPost]
    public async Task<ActionResult<HotelResponse>> CreateHotel(CreateHotelRequest request)
    {
        var hotel = new Hotel
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Location = request.Location,
            Rating = request.Rating,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            Status = HotelStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, new HotelResponse(hotel.Id, hotel.Name, hotel.Location, hotel.Rating, hotel.Status.ToString(), 0, hotel.ImageUrl));
    }
}
