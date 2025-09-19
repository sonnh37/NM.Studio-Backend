using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.Options;

namespace NM.Studio.Services;

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;

    public EmailService(IOptions<EmailOptions> emailSettings)
    {
        _emailOptions = emailSettings.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient(_emailOptions.SmtpServer, _emailOptions.Port)
        {
            Credentials = new NetworkCredential(_emailOptions.SenderEmail, _emailOptions.Password),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailOptions.SenderEmail, _emailOptions.SenderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(to);

        await client.SendMailAsync(mailMessage);
    }
}