using CoworkingManagement.Application.Common.Models;
using MediatR;

namespace CoworkingManagement.Application.Features.Rooms.Queries.GetRoomsList;

public record GetRoomsListQuery : IRequest<List<RoomDto>>;