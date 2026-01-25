using CoworkingManagement.Application.Common.Models;
using MediatR;

namespace CoworkingManagement.Application.Features.Reservations.Queries.GetReservationsList;

public record GetReservationsListQuery : IRequest<List<ReservationDto>>;