using MediatR;

namespace CoworkingManagement.Application.Features.Reservations.Commands.UpdateReservation;

public record UpdateReservationCommand(
    Guid ReservationId,
    Guid? RoomId,
    DateTime? StartDate,
    DateTime? EndDate
): IRequest<Unit>;