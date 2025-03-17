using System.Net.Mail;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public async Task SecurityCode(UserResetRequest request)
    {
        _logger.LogDebug("Generate security code for {user}", request.Email);
        var options = _upperCaseLetters + _digits;
        var user = await CheckUserAccount(_protector.ProtectEmail(request.Email));
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
            LogEvents.OPERATION_SUCCESS,
            "New security code has been sent to a user"
        );
        user.SecurityCode = _protector.Protect(code);
        user.SecurityCodeCreatedAt = DateTime.UtcNow;
        await UpdateUser(user);
    }
}
