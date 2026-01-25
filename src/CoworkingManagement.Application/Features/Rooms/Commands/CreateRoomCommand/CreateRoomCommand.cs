using MediatR;

namespace CoworkingManagement.Application.Features.Rooms.Commands.CreateRoomCommand;

public record CreateRoomCommand(int Capacity, string Location) : IRequest<Guid>;