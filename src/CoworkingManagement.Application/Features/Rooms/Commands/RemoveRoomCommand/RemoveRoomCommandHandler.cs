using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Domain.Enums;
using CoworkingManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Rooms.Commands.RemoveRoomCommand;

internal sealed class RemoveRoomCommandHandler(IApplicationDbContext context, ICacheService cache) : IRequestHandler<RemoveRoomCommand, Unit>
{
    private readonly IApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ICacheService _cache = cache ?? throw new ArgumentNullException(nameof(cache));

    public async Task<Unit> Handle(RemoveRoomCommand command, CancellationToken cancellationToken)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == command.RoomId, cancellationToken);

        if (room == null)
        {
            throw new NotFoundException(nameof(room), command.RoomId);
        }

        var hasActiveReservations = await _context.Reservations
                                        .AnyAsync(r => r.RoomId == command.RoomId && 
                                        r.Status == ReservationStatus.Reserved && 
                                        r.EndDate >= DateTime.UtcNow, cancellationToken: cancellationToken);

        if (hasActiveReservations)
        {
            throw new BusinessException("Cannot remove a room with active or upcoming reservations.");
        }

        room.Delete();
        
        await _context.SaveChangesAsync(cancellationToken);

        _cache.Invalidate("Rooms");
        return Unit.Value;
    }
}