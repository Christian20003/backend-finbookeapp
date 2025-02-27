using System.Net.Mail;
using FinBookeAPI.Models.Authentication.Interfaces;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    private const string _upperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string _lowerCaseLetters = "abcdefghijklmnopqrstuvwxyz";
    private const string _digits = "0123456789";
    private const string _specialChars = "!§$%&/()=?{[]}";

    public async Task ResetPassword(IUserResetRequest request)
    {
        var user = await CheckUserAccount(_protector.Protect(request.Email));
        if (user.SecurityCode == null || user.SecurityCodeCreatedAt == null || request.Code == null)
        {
            throw new AuthenticationException(
                "Security code is not set properly",
                ErrorCodes.INVALID_ENTRY
            );
        }
        var timeWindow = user.SecurityCodeCreatedAt.Value.AddMinutes(10);
        if (DateTime.UtcNow.Ticks - timeWindow.Ticks > 0)
        {
            throw new AuthenticationException(
                "Generated security code is not valid anymore",
                ErrorCodes.UNAUTHORIZED
            );
        }
        if (user.SecurityCode != _protector.Protect(request.Code))
        {
            throw new AuthenticationException(
                "Received security code is not correct",
                ErrorCodes.UNAUTHORIZED
            );
        }
        var options = _upperCaseLetters + _lowerCaseLetters + _digits + _specialChars;
        var password = GenerateRandomString(options, 20);
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, password);
        if (!result.Succeeded)
        {
            throw new AuthenticationException("Reseting password failed", ErrorCodes.UPDATE_FAILED);
        }
        var message = new MailMessage
        {
            From = new MailAddress("noreply@finbooke.com"),
            Subject = "Requested security code",
            // TODO: Make it better
            Body =
                "<h1>Hello</h1><p>Here is your requested security code</p><h5>"
                + password
                + "</h5>",
            IsBodyHtml = true,
        };
        message.To.Add(request.Email);
        SendEmail(message);
    }
}
