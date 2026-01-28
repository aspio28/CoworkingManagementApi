using System.Text.Json.Serialization;
using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Application.Common.Models;
using MediatR;

namespace CoworkingManagement.Application.Features.Reservations.Queries.GetReservationsList;

public record GetReservationsListQuery(
    bool OnlyActive = true,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PaginatedList<ReservationDto>>, ICacheableQuery
{
    [JsonIgnore]
    public Guid UserId { get; init; }

    [JsonIgnore]
    public string CacheKey => $"GetReservations{UserId}_{PageNumber}_{PageSize}_{OnlyActive}";
    
    [JsonIgnore]
    public string CacheTag => "Reservations";

    [JsonIgnore]
    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

}