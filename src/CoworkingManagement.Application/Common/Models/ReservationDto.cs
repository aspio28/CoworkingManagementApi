using CoworkingManagement.Domain.Enums;

namespace CoworkingManagement.Application.Common.Models;

public record ReservationDto : BaseEntityDto
{
    public Guid RoomId { get; init; }
    public Guid UserId { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public ReservationStatus Status { get; init; }
    public string RoomLocation { get; init; } = string.Empty;
    public int RoomCapacity { get; init; }
    public DateTime? CanceledAt { get; set; }
}