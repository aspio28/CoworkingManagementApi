using CoworkingManagement.Application.Common.Interfaces;
using MediatR;

namespace CoworkingManagement.Application.Features.Reservations.Events.ReservationCancelled;
public class ReservationCancelledEmailHandler
    : INotificationHandler<ReservationCancelledEvent>
{
    private readonly IEmailSender _emailSender;

    public ReservationCancelledEmailHandler(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task Handle(
        ReservationCancelledEvent notification,
        CancellationToken ct)
    {
        var body = $"""
            <h2>Reserva cancelada</h2>
            <p>La reserva existente:</p>
            <p>
                Desde {notification.StartDate:dd/MM/yyyy}
                hasta {notification.EndDate:dd/MM/yyyy}
            </p>
            <p>Ha sido cancelada</>
        """;

        await _emailSender.SendAsync(
            notification.Email!,
            "Reserva cancelada",
            body,
            ct);
    }
}