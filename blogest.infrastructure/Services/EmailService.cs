using blogest.application.Interfaces.services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace blogest.infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        MimeMessage email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_configuration["Smtp:UserName"]));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", int.Parse(_configuration["Smtp:Port"]), SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(
            _configuration["Smtp:UserName"],
            _configuration["Smtp:Password"]
        );
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}