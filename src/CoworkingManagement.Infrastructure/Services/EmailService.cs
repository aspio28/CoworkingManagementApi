using System.Net;
using System.Net.Mail;
using CoworkingManagement.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CoworkingManagement.Infrastructure.Services;

public class EmailService : IEmailSender
{
    private readonly SmtpClient _client;
    private readonly string _from;

    public EmailService(IConfiguration config)
    {
        _from = config["Email:From"]!;

        _client = new SmtpClient(config["Email:Smtp:Host"])
        {
            Port = int.Parse(config["Email:Smtp:Port"]!),
            Credentials = new NetworkCredential(
                config["Email:Smtp:User"],
                config["Email:Smtp:Password"]
            ),
            EnableSsl = true
        };
    }

    public async Task SendAsync(
        string to,
        string subject,
        string htmlBody,
        CancellationToken ct = default)
    {
        var mail = new MailMessage(_from, to, subject, htmlBody)
        {
            IsBodyHtml = true
        };

        await _client.SendMailAsync(mail, ct);
    }
}