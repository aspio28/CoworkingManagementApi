using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Rooms.Queries.GetRoomsList;

internal sealed class GetRoomsListHandler : IRequestHandler<GetRoomsListQuery, List<RoomDto>>
{
    private readonly IApplicationDbContext _context;

    public GetRoomsListHandler(IApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<RoomDto>> Handle(GetRoomsListQuery request, CancellationToken cancellationToken)
    {
        return await _context.Rooms
            .AsNoTracking()
            .Select(room => new RoomDto
            {
                Id = room.Id,
                Capacity = room.Capacity,
                Location = room.Location,
                Status = room.Status.ToString()
            })
            .ToListAsync(cancellationToken);
    }
}