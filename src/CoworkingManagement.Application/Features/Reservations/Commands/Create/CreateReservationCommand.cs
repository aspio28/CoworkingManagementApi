using MediatR;

namespace CoworkingManagement.Application.Features.Reservations.Commands.Create;

public record CreateReservationCommand(
    Guid RoomId,
    Guid UserId,
    DateTime Date,
    TimeSpan StartTime,
    TimeSpan EndTime
): IRequest<Unit>;