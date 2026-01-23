using MediatR;

namespace CoworkingManagement.Application.Features.Reservations.Commands.RemoveReservation;

public record RemoveReservationCommand(
    Guid ReservationId
): IRequest<Unit>;