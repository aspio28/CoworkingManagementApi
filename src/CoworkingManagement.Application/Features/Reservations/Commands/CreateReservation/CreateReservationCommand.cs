using MediatR;

namespace CoworkingManagement.Application.Features.Reservations.Commands.CreateReservation;

public record CreateReservationCommand(
    Guid RoomId,
    Guid UserId,
    DateTime StartDate,
    DateTime EndDate
): IRequest<Guid>;