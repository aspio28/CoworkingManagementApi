using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Application.Common.Models;
using CoworkingManagement.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Reservations.Queries.GetReservationById;

internal sealed class GetReservationByIdHandler : IRequestHandler<GetReservationByIdQuery, ReservationDto>
{
    private readonly IApplicationDbContext _context;

    public GetReservationByIdHandler(IApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<ReservationDto> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
    {
        var reservation = await _context.Reservations
            .AsNoTracking()
            .Include(r => r.Room)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (reservation == null)
        {
            throw new NotFoundException(nameof(reservation), request.Id);
        }

        return new ReservationDto
        {
            Id = reservation.Id,
            RoomId = reservation.RoomId,
            UserId = reservation.UserId,
            StartDate = reservation.StartDate,
            EndDate = reservation.EndDate,
            Status = reservation.Status,
            RoomCapacity = reservation.Room.Capacity,
            RoomLocation = reservation.Room.Location,
            CreatedAt = reservation.CreatedAt, 
            CreatedBy = reservation.CreatedBy, 
            LastModifiedAt = reservation.LastModifiedAt, 
            LastModifiedBy = reservation.LastModifiedBy,
            CanceledAt = reservation.CancelledAt,
        };
    }
}