using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Application.Common.Models;
using CoworkingManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Rooms.Queries.GetRoomsList;

internal sealed class GetRoomsListHandler : IRequestHandler<GetRoomsListQuery, PaginatedList<RoomDto>>
{
    private readonly IApplicationDbContext _context;

    public GetRoomsListHandler(IApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<PaginatedList<RoomDto>> Handle(GetRoomsListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Rooms.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(r => r.Location.Contains(request.SearchTerm));
        }

        if (request.MinCapacity.HasValue)
        {
            query = query.Where(r => r.Capacity >= request.MinCapacity.Value);
        }

        if (request.StartDate.HasValue && request.EndDate.HasValue)
        {
            var start = request.StartDate.Value;
            var end = request.EndDate.Value;

            query = query.Where(room => !_context.Reservations.Any(res => 
                res.RoomId == room.Id &&
                res.Status == ReservationStatus.Reserved &&
                start < res.EndDate && 
                end > res.StartDate
            ));
        }
        var count = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(r => new RoomDto { 
                Id = r.Id, 
                Capacity = r.Capacity, 
                Location = r.Location, 
                CreatedAt = r.CreatedAt, 
                CreatedBy = r.CreatedBy, 
                LastModifiedAt = r.LastModifiedAt, 
                LastModifiedBy = r.LastModifiedBy
            })
            .ToListAsync(cancellationToken);

        return new PaginatedList<RoomDto>(items, count, request.PageNumber, request.PageSize);
    }
}