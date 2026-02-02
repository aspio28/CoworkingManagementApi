using MediatR;

namespace CoworkingManagement.Application.Features.Reservations.Events.ReservationConfirmed;

public record ReservationConfirmedEvent
(
    string? Email,
    DateTime StartDate,
    DateTime EndDate
) : INotification;