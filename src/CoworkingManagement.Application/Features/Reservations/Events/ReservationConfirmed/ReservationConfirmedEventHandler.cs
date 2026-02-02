using CoworkingManagement.Application.Common.Interfaces;
using MediatR;

namespace CoworkingManagement.Application.Features.Reservations.Events.ReservationConfirmed;
public class ReservationConfirmedEmailHandler
    : INotificationHandler<ReservationConfirmedEvent>
{
    private readonly IEmailSender _emailSender;

    public ReservationConfirmedEmailHandler(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task Handle(
        ReservationConfirmedEvent notification,
        CancellationToken ct)
    {
        var body = $"""
            <h2>Reserva confirmada</h2>
            <p>Tu reserva ha sido confirmada.</p>
            <p>
                Desde {notification.StartDate:dd/MM/yyyy}
                hasta {notification.EndDate:dd/MM/yyyy}
            </p>
        """;

        await _emailSender.SendAsync(
            notification.Email!,
            "Reserva confirmada",
            body,
            ct);
    }
}