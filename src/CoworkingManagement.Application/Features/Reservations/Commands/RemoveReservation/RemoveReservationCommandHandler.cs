using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Reservations.Commands.RemoveReservation;

internal sealed class RemoveReservationCommandHandler(IApplicationDbContext context): IRequestHandler<RemoveReservationCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;
    public async Task<Unit> Handle(RemoveReservationCommand command, CancellationToken cancellationToken)
    {
        var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == command.ReservationId, cancellationToken);

        if(reservation == null)
        {
            throw new NotFoundException(nameof(reservation), command.ReservationId);
        }

        else if (reservation.Date < DateTime.Today)
        {
            throw new BusinessException("Cannot remove past reservations.");
        }

        else if (reservation.Date == DateTime.Today && reservation.StartTime <= DateTime.Now.TimeOfDay)
        {
            throw new BusinessException("Cannot remove ongoing or past reservations.");
        }

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}