namespace Altairis.Domain.Entities;

public enum HotelStatus
{
    Active,
    Maintenance,
    Inactive
}

public class Hotel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int Rating { get; set; }
    public HotelStatus Status { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int TotalRooms { get; set; } // Puede ser calculado o manual
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<RoomType> RoomTypes { get; set; } = new List<RoomType>();
}
