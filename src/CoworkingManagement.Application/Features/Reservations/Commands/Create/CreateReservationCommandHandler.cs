using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Domain.Entities;
using CoworkingManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Reservations.Commands.Create;

internal sealed class CreateReservationCommandHandler(IApplicationDbContext context): IRequestHandler<CreateReservationCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;
    public async Task<Unit> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
    {

        bool isRoomOccupied = await _context.Reservations.AnyAsync(r => r.RoomId == command.RoomId && 
                       r.Date == command.Date &&
                       ((command.StartTime >= r.StartTime && command.StartTime < r.EndTime) ||
                        (command.EndTime > r.StartTime && command.EndTime <= r.EndTime) ||
                        (r.StartTime >= command.StartTime && r.StartTime < command.EndTime)),
                  cancellationToken);

    if (isRoomOccupied)
    {
        throw new BusinessException("Room is already booked for the selected time slot.");
    }
        Reservation reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomId = command.RoomId,
            UserId = command.UserId,
            Date = command.Date,
            StartTime = command.StartTime,
            EndTime = command.EndTime
        };
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}