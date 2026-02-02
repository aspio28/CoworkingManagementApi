using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Application.Features.Reservations.Events.ReservationCancelled;
using CoworkingManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Reservations.Commands.CancelReservation;

internal sealed class CancelReservationCommandHandler(IApplicationDbContext context, ICacheService cache, ICurrentUserService currentUserService, IMediator mediator): IRequestHandler<CancelReservationCommand, Unit>
{
    private readonly IApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ICacheService _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    private readonly ICurrentUserService _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
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

        var email = _currentUserService.Email;

        await _mediator.Publish(new ReservationCancelledEvent(   
            email!,
            reservation.StartDate,
            reservation.EndDate
        ), cancellationToken);

        _cache.Invalidate("Rooms");
        _cache.Invalidate("Reservations");
        return Unit.Value;
    }
}