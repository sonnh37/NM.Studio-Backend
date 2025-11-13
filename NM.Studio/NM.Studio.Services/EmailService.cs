using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NM.Studio.Domain.Contracts.Services;
using NM.Studio.Domain.Models.Options;
using NM.Studio.Domain.Shared.Exceptions;

namespace NM.Studio.Services;

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailOptions> emailSettings, ILogger<EmailService> logger)
    {
        _emailOptions = emailSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        if (string.IsNullOrEmpty(to))
        {
            throw new DomainException("Invalid email address");
        }
        
        using var client = new SmtpClient(_emailOptions.SmtpServer, _emailOptions.Port)
        {
            Port = _emailOptions.Port,
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