using MediatR;

namespace CoworkingManagement.Application.Features.Reservations.Commands.CancelReservation;

public record CancelReservationCommand(
    Guid ReservationId
): IRequest<Unit>;