using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Domain.Entities;
using CoworkingManagement.Domain.Enums;
using CoworkingManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Reservations.Commands.UpdateReservation;

internal sealed class UpdateReservationCommandHandler(IApplicationDbContext context): IRequestHandler<UpdateReservationCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> Handle(UpdateReservationCommand command, CancellationToken cancellationToken)
    {
        var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == command.ReservationId, cancellationToken);

        if(reservation == null)
        {
            throw new NotFoundException(nameof(reservation), command.ReservationId);
        }

        bool isRoomOccupied = await _context.Reservations.AnyAsync(r => r.RoomId == command.RoomId && 
                        r.Status != ReservationStatus.Cancelled && 
                        command.StartDate < r.EndDate && command.EndDate > r.StartDate,
                    cancellationToken);

        if (isRoomOccupied)
        {
            throw new BusinessException("Room is already booked for the selected time slot.");
        }

        reservation.Update(
            command.RoomId,
            command.UserId,
            command.StartDate,
            command.EndDate
        );

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}