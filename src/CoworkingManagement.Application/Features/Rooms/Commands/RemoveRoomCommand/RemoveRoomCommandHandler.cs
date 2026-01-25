using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Rooms.Commands.RemoveRoomCommand;

internal sealed class RemoveRoomCommandHandler: IRequestHandler<RemoveRoomCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public RemoveRoomCommandHandler(IApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Unit> Handle(RemoveRoomCommand command, CancellationToken cancellationToken)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == command.RoomId, cancellationToken);

        if (room == null)
        {
            throw new NotFoundException(nameof(room), command.RoomId);
        }

        if (room.Reservations.Any(r => r.Status != Domain.Enums.ReservationStatus.Cancelled && r.EndDate >= DateTime.UtcNow))
        {
            throw new BusinessException("Cannot remove a room with active or upcoming reservations.");
        }

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}