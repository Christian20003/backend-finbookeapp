using System.Net.Mail;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    private const string _upperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string _lowerCaseLetters = "abcdefghijklmnopqrstuvwxyz";
    private const string _digits = "0123456789";
    private const string _specialChars = "!§$%&/()=?{[]}";

    public async Task ResetPassword(UserResetRequest request)
    {
        _logger.LogDebug(
            "Reset password call with provided security code for {user}",
            request.Email
        );
        var user = await CheckUserAccount(request.Email);
        if (user.SecurityCode == null || user.SecurityCodeCreatedAt == null || request.Code == null)
        {
            _logger.LogWarning(
                LogEvents.PROPERTY_MISSING,
                "Invalid security code provided or stored"
            );
            throw new AuthenticationException(
                ErrorCodes.UNEXPECTED_STRUCTURE,
                "Security code is not set properly"
            );
        }
        var timeWindow = user.SecurityCodeCreatedAt.Value.AddMinutes(10);
        if (DateTime.UtcNow.Ticks - timeWindow.Ticks > 0)
        {
            _logger.LogWarning(LogEvents.PROPERTY_TOO_SMALL, "Lifetime of security code exceeded");
            throw new AuthenticationException(
                ErrorCodes.EXPIRED_CODE,
                "Generated security code is not valid anymore"
            );
        }
        if (user.SecurityCode != _protector.Protect(request.Code))
        {
            _logger.LogWarning(LogEvents.PROPERTY_UNEQUAL, "Provided security code is invalid");
            throw new AuthenticationException(
                ErrorCodes.INVALID_CODE,
                "Received security code is not correct"
            );
        }
        var options = _upperCaseLetters + _lowerCaseLetters + _digits + _specialChars;
        var password = GenerateRandomString(options, 20);
        var result = await _accountManager.SetPasswordAsync(user, password);
        if (!result.Succeeded)
        {
            _logger.LogWarning(LogEvents.UPDATE_FAILED, "Password could not be reset");
            throw new AuthenticationException(ErrorCodes.UPDATE_FAILED, "Reseting password failed");
        }
        user.SecurityCode = null;
        user.SecurityCodeCreatedAt = null;
        await UpdateUser(user);
        var message = new MailMessage
        {
            From = new MailAddress("noreply@finbooke.com"),
            Subject = "Requested security code",
            Body = Email.GetPasswordEmail(password),
            IsBodyHtml = true,
        };
        message.To.Add(request.Email);
        SendEmail(message);
    }
}
