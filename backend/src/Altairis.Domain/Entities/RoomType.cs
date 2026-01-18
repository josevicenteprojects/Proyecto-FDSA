namespace Altairis.Domain.Entities;

public class RoomType
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public int Capacity { get; set; }
    public int Stock { get; set; } // Capacidad física máxima

    public Hotel? Hotel { get; set; }
    public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
