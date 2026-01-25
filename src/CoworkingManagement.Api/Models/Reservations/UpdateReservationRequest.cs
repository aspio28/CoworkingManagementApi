namespace CoworkingManagement.Api.Models.Reservations;

public record UpdateReservationRequest(
    Guid? RoomId,
    DateTime? StartDate,
    DateTime? EndDate
);