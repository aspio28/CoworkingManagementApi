using MediatR;

namespace CoworkingManagement.Application.Features.Rooms.Commands.RemoveRoomCommand;

public record RemoveRoomCommand(Guid RoomId) : IRequest<Unit>;