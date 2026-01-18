namespace Altairis.Domain.Entities;

public enum BookingStatus
{
    Confirmed,
    Cancelled,
    CheckedIn,
    CheckedOut
}

public class Booking
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public Guid RoomTypeId { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public BookingStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation props (optional for MVP but good for structure)
    // public Hotel? Hotel { get; set; }
    // public RoomType? RoomType { get; set; }
}
