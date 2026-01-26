namespace CoworkingManagement.Application.Common.Models;

public record RoomDto : BaseEntityDto
{
    public int Capacity { get; init; }
    public string Location { get; init; } = string.Empty;
}