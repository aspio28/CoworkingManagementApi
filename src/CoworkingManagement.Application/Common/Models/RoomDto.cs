namespace CoworkingManagement.Application.Common.Models;

public record RoomDto
{
    public Guid Id { get; init; }
    public int Capacity { get; init; }
    public string Location { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
}