
namespace blogest.application.Interfaces.services;

public interface IEmailService
{
    public Task SendEmailAsync(string toEmail, string subject, string body);
}