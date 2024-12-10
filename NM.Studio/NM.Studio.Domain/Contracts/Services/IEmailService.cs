namespace NM.Studio.Domain.Contracts.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}