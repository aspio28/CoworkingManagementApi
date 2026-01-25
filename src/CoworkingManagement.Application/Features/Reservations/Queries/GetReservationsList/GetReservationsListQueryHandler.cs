using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Reservations.Queries.GetReservationsList;

internal sealed class GetReservationsListQueryHandler : IRequestHandler<GetReservationsListQuery, List<ReservationDto>>
{
    private readonly IApplicationDbContext _context;

    public GetReservationsListQueryHandler(IApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<ReservationDto>> Handle(GetReservationsListQuery request, CancellationToken cancellationToken)
    {
        return await _context.Reservations
            .AsNoTracking()
            .Select(reservation => new ReservationDto
            {
                Id = reservation.Id,
                RoomId = reservation.RoomId,
                UserId = reservation.UserId,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,  
                Status = reservation.Status.ToString()
            })
            .ToListAsync(cancellationToken);
    }
}