using System.Net;
using System.Net.Mail;
using FinBookeAPI.Models.Authentication.Interfaces;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public async Task SecurityCode(IUserResetRequest request)
    {
        _logger.LogDebug("Check existence of {user}", request.Email);
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
        var user = await CheckUserAccount(_protector.Protect(request.Email));
        Random res = new();
        var code = "";

        _logger.LogDebug("Generate new security code");
        for (int i = 0; i < 6; i++)
        {
            code += chars[res.Next(36)];
        }
        var server = _mailServer.Value;
        var message = new MailMessage
        {
            From = new MailAddress("noreply@finbooke.com"),
            Subject = "Requested security code",
            // TODO: Make it better
            Body = "<h1>Hello</h1><p>Here is your requested security code</p><h5>" + code + "</h5>",
            IsBodyHtml = true,
        };
        message.To.Add(request.Email);
        var smtpClient = new SmtpClient
        {
            Host = server.Host,
            Port = server.Port,
            Credentials = new NetworkCredential(server.Username, server.Password),
            EnableSsl = true,
        };
        try
        {
            _logger.LogDebug("Send code to user");
            smtpClient.Send(message);
            _logger.LogInformation(
                LogEvents.SUCCESSFUL_OPERATION,
                "New security code has been sent to a user"
            );
            user.SecurityCode = _protector.Protect(code);
            user.SecurityCodeCreatedAt = DateTime.UtcNow;
            _logger.LogDebug("Update user account with new security code");
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError(
                    LogEvents.FAILED_UPDATE,
                    "User account could not be updated with security code"
                );
                throw new AuthenticationException(
                    "Security code could not be stored in the database",
                    ErrorCodes.UPDATE_FAILED
                );
            }
        }
        catch (Exception exception)
            when (exception is InvalidOperationException
                || exception is ObjectDisposedException
                || exception is SmtpException
                || exception is SmtpFailedRecipientException
                || exception is SmtpFailedRecipientsException
            )
        {
            _logger.LogError(LogEvents.FAILED_OPERATION, "SMTP-Server does not sent email");
            throw new AuthenticationException(
                "Mail with security code could not be sent",
                ErrorCodes.SERVER_ERROR,
                exception
            );
        }
    }
}
