using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Rooms.Commands.UpdateRoomCommand;

internal sealed class UpdateRoomCommandHandler(IApplicationDbContext context, ICacheService cache) : IRequestHandler<UpdateRoomCommand, Unit>
{
    private readonly IApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ICacheService _cache = cache ?? throw new ArgumentNullException(nameof(cache));

    public async Task<Unit> Handle(UpdateRoomCommand command, CancellationToken cancellationToken)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == command.RoomId, cancellationToken);

        if (room == null)
        {
            throw new NotFoundException(nameof(room), command.RoomId);
        }

        if (room.Reservations.Any(r => r.Status != Domain.Enums.ReservationStatus.Cancelled && r.EndDate >= DateTime.UtcNow))
        {
            throw new BusinessException("Cannot update a room with active or upcoming reservations.");
        }

        room.Update(
            capacity: command.Capacity,
            location: command.Location
        );

        _context.Rooms.Update(room);
        await _context.SaveChangesAsync(cancellationToken);

        _cache.Invalidate("Rooms");
        return Unit.Value;
    }
}