using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Reservations.Commands.CancelReservation;

internal sealed class CancelReservationCommandHandler(IApplicationDbContext context): IRequestHandler<CancelReservationCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;
    public async Task<Unit> Handle(CancelReservationCommand command, CancellationToken cancellationToken)
    {
        var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == command.ReservationId, cancellationToken);

        if(reservation == null)
        {
            throw new NotFoundException(nameof(reservation), command.ReservationId);
        }

        else if (reservation.EndDate < DateTime.UtcNow)
        {
            throw new BusinessException("Cannot remove past reservations.");
        }

        else if (reservation.StartDate <= DateTime.UtcNow)
        {
            throw new BusinessException("Cannot remove ongoing or past reservations.");
        }

        reservation.Cancel();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}