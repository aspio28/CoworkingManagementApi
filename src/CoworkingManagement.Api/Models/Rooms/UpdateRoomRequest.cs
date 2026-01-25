namespace CoworkingManagement.Api.Models.Rooms;

public record UpdateRoomRequest(
    int? Capacity,
    string? Location
);