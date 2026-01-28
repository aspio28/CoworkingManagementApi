using CoworkingManagement.Application.Common.Interfaces;
using CoworkingManagement.Application.Common.Models;
using CoworkingManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoworkingManagement.Application.Features.Reservations.Queries.GetReservationsList;

internal sealed class GetReservationsListQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService) : IRequestHandler<GetReservationsListQuery, PaginatedList<ReservationDto>>
{
    private readonly IApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    private readonly ICurrentUserService _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));

    public async Task<PaginatedList<ReservationDto>> Handle(GetReservationsListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Reservations.AsNoTracking().Where(r => r.UserId == _currentUserService.UserId);

        if(request.OnlyActive)
        {
            query = query.Where(r => r.Status == ReservationStatus.Reserved);
        }
        
        var count = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(r => new ReservationDto 
            { 
                Id = r.Id, 
                RoomId = r.RoomId, 
                UserId = r.UserId, 
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                Status = r.Status,
                CreatedAt = r.CreatedAt, 
                CreatedBy = r.CreatedBy, 
                LastModifiedAt = r.LastModifiedAt, 
                LastModifiedBy = r.LastModifiedBy,
                CanceledAt = r.CancelledAt,
            })
            .ToListAsync(cancellationToken);
        
        return new PaginatedList<ReservationDto>(items, count, request.PageNumber, request.PageSize);
    }
}