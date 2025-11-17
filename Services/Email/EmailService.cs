using System.Net;
using System.Net.Mail;
using FinBookeAPI.Models.Configuration;
using Microsoft.Extensions.Options;

namespace FinBookeAPI.Services.Email;

public class EmailService(IOptions<SmtpServer> config, ILogger<EmailService> logger) : IEmailService
{
    private readonly IOptions<SmtpServer> _config = config;
    private readonly ILogger<EmailService> _logger = logger;

    public void Send(string email, string subject, string body, bool isHtml = true)
    {
        _logger.LogDebug("Sending email to {email}", email);
        var host = _config.Value.Host;
        var from = _config.Value.Address;
        var port = _config.Value.Port;
        if (host == null)
        {
            _logger.LogError(LogEvents.ConfigurationError, "SMTP-Host is null");
            throw new ApplicationException("SMTP-Host cannot be null");
        }
        if (from == null)
        {
            _logger.LogError(LogEvents.ConfigurationError, "Email-Address of sender is null");
            throw new ApplicationException("Address of the sender cannot be null");
        }
        if (port <= 0 || port > 65535)
        {
            _logger.LogError(LogEvents.ConfigurationError, "SMTP-Port is invalid: {port}", port);
            throw new ApplicationException("SMTP-Port must be between 1 and 65.535");
        }
        try
        {
            var message = new MailMessage
            {
                From = new MailAddress(from),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml,
            };
            message.To.Add(email);
            var smtpClient = new SmtpClient
            {
                Host = host,
                Port = port,
                Credentials = new NetworkCredential(_config.Value.Username, _config.Value.Password),
                EnableSsl = true,
            };
            smtpClient.Send(message);
        }
        catch (Exception exception)
            when (exception is ArgumentException || exception is FormatException)
        {
            _logger.LogError(
                LogEvents.ConfigurationError,
                "Email-Address of sender is not a valid email address"
            );
            throw new ApplicationException("Address of the sender is not a valid email address");
        }
    }
}
