namespace Altairis.Domain.Entities;

public class Inventory
{
    public Guid Id { get; set; }
    public Guid RoomTypeId { get; set; }
    public DateOnly Date { get; set; }
    public int Available { get; set; }
    public decimal Price { get; set; }

    public RoomType? RoomType { get; set; }
}
