using System.Text.Json.Serialization;
using CoworkingManagement.Application.Common.Interfaces;
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
) : IRequest<PaginatedList<RoomDto>>, ICacheableQuery
{
    [JsonIgnore]
    public string CacheKey => $"GetRooms_{PageNumber}_{PageSize}_{SearchTerm}_{MinCapacity}_{StartDate}_{EndDate}";
    
    [JsonIgnore]
    public string CacheTag => "Rooms";
    
    [JsonIgnore]
    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
}