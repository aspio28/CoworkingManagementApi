using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Domain.Entities;
using CoworkingManagement.Domain.Enums;
using CoworkingManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Reservations.Commands.CreateReservation;

internal sealed class CreateReservationCommandHandler(IApplicationDbContext context): IRequestHandler<CreateReservationCommand, Guid>
{
    private readonly IApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    public async Task<Guid> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
    {

        bool isRoomOccupied = await _context.Reservations.AnyAsync(r => r.RoomId == command.RoomId && 
                        r.Status != ReservationStatus.Cancelled && 
                        command.StartDate < r.EndDate && command.EndDate > r.StartDate,
                    cancellationToken);

        if (isRoomOccupied)
        {
            throw new BusinessException("Room is already booked for the selected time slot.");
        }
        Reservation reservation = new Reservation
        (
            command.RoomId,
            command.UserId,
            ReservationStatus.Reserved,
            command.StartDate,
            command.EndDate
        );
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync(cancellationToken);

        return reservation.Id;
    }
}