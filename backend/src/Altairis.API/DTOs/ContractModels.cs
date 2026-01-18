namespace Altairis.API.DTOs;

// Hoteles
public record CreateHotelRequest(string Name, string Location, int Rating, string? Description, string? ImageUrl);
public record HotelResponse(Guid Id, string Name, string Location, int Rating, string Status, int TotalRooms, string? ImageUrl);
public record HotelDetailResponse(Guid Id, string Name, string Location, int Rating, string Status, string Description, string? ImageUrl, IEnumerable<RoomTypeResponse> RoomTypes);

// Tipos de Habitación
public record CreateRoomTypeRequest(Guid HotelId, string Name, decimal BasePrice, int Capacity, int Stock);
public record RoomTypeResponse(Guid Id, Guid HotelId, string Name, decimal BasePrice, int Capacity, int Stock);

// Inventario
public record InventoryResponse(Guid RoomTypeId, DateOnly Date, int Available, decimal Price);
public record UpdateInventoryRequest(Guid RoomTypeId, DateOnly Date, int Available, decimal Price);
public record BatchInventoryRequest(Guid HotelId, DateOnly StartDate, DateOnly EndDate, decimal? PriceModifier); // Lógica de lote de ejemplo

// Reservas
public record CreateBookingRequest(Guid HotelId, Guid RoomTypeId, string GuestName, DateOnly CheckIn, DateOnly CheckOut);
public record BookingResponse(
    string Id,
    string GuestName,
    string HotelName,
    string RoomTypeName,
    DateOnly CheckIn,
    DateOnly CheckOut,
    string Status,
    decimal TotalPrice
);

public record DashboardStatsResponse(
    int ActiveHotels,
    int TotalBookings,
    int TotalGuests,
    double OccupancyRate
);

public record PagedResponse<T>(IEnumerable<T> Items, int TotalCount, int Page, int PageSize);
