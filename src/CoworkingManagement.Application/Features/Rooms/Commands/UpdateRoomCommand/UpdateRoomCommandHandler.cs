using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Rooms.Commands.UpdateRoomCommand;

internal sealed class UpdateRoomCommandHandler: IRequestHandler<UpdateRoomCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateRoomCommandHandler(IApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

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

        return Unit.Value;
    }
}