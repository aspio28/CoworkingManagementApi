using MediatR;

namespace CoworkingManagement.Application.Features.Rooms.Commands.UpdateRoomCommand;

public record UpdateRoomCommand(
    Guid RoomId,
    int? Capacity,
    string? Location
): IRequest<Unit>;