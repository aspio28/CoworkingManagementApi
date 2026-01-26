using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Domain.Entities;
using CoworkingManagement.Domain.Enums;
using CoworkingManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Reservations.Commands.CreateReservation;

internal sealed class CreateReservationCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService): IRequestHandler<CreateReservationCommand, Guid>
{
    private readonly IApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ICurrentUserService _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
    public async Task<Guid> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
    {

        var userId = _currentUserService.UserId;

        if (!userId.HasValue)
            throw new UnauthorizedException("Usuario no identificado");

        bool isRoomOccupied = await _context.Reservations.AnyAsync(r => r.RoomId == command.RoomId && 
                        r.Status == ReservationStatus.Reserved && 
                        command.StartDate < r.EndDate && command.EndDate > r.StartDate,
                    cancellationToken);

        if (isRoomOccupied)
        {
            throw new BusinessException("Room is already booked for the selected time slot.");
        }
        Reservation reservation = new(
            userId: userId.Value,
            roomId: command.RoomId,
            status: ReservationStatus.Reserved,
            startDate: command.StartDate,
            endDate: command.EndDate
        );
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync(cancellationToken);

        return reservation.Id;
    }
}