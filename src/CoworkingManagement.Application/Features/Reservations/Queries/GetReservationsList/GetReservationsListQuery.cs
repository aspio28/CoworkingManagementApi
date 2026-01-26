using CoworkingManagement.Application.Common.Models;
using MediatR;

namespace CoworkingManagement.Application.Features.Reservations.Queries.GetReservationsList;

public record GetReservationsListQuery(
    bool OnlyActive = true,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PaginatedList<ReservationDto>>;