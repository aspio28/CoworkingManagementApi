using CoworkingManagement.Application.Common.Models;
using MediatR;

namespace CoworkingManagement.Application.Features.Rooms.Queries.GetRoomsList;

public record GetRoomsListQuery(
    string? SearchTerm, 
    int? MinCapacity, 
    DateTime? StartDate,
    DateTime? EndDate,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PaginatedList<RoomDto>>;