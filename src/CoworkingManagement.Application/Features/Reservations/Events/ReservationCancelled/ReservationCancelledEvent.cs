using MediatR;

namespace CoworkingManagement.Application.Features.Reservations.Events.ReservationCancelled;

public record ReservationCancelledEvent
(
    string Email,
    DateTime StartDate, 
    DateTime EndDate     

) : INotification;