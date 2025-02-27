using System.Net.Mail;
using FinBookeAPI.Models.Authentication.Interfaces;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public async Task SecurityCode(IUserResetRequest request)
    {
        _logger.LogDebug("Generate security code for {user}", request.Email);
        var options = _upperCaseLetters + _digits;
        var user = await CheckUserAccount(_protector.Protect(request.Email));
        var code = GenerateRandomString(options, 6);
        var message = new MailMessage
        {
            From = new MailAddress("noreply@finbooke.com"),
            Subject = "Requested security code",
            Body = Email.GetCodeBody(code),
            IsBodyHtml = true,
        };
        message.To.Add(request.Email);
        SendEmail(message);
        _logger.LogInformation(
            LogEvents.SUCCESSFUL_OPERATION,
            "New security code has been sent to a user"
        );
        user.SecurityCode = _protector.Protect(code);
        user.SecurityCodeCreatedAt = DateTime.UtcNow;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            _logger.LogWarning(
                LogEvents.FAILED_UPDATE,
                "User account could not be updated with security code"
            );
            throw new AuthenticationException(
                "Security code could not be stored in the database",
                ErrorCodes.UPDATE_FAILED
            );
        }
    }
}
