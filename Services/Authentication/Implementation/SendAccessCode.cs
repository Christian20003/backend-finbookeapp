using FinBookeAPI.AppConfig.Redaction;
using FinBookeAPI.Models.Configuration;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    private const string TemplateFileCode = "../../../Templates/Email/AccessCode.html";

    public async Task SendAccessCode(string email)
    {
        _logger.LogDebug(
            "Generate new access code for {email}",
            PrivacyGuard.Hide(_redactor, email)
        );
        if (!VerifyEmail(email))
        {
            _logger.LogWarning(
                LogEvents.ResetCredentialsFailed,
                "{email} is not a valid email address",
                PrivacyGuard.Hide(_redactor, email)
            );
            throw new ArgumentException($"{email} is not a valid email address", nameof(email));
        }
        var user = await VerifyUserAccount(email);
        var code = _securityUtilityService.GenerateAccessCode(6);
        var subject = "Requested security code";
        string? body;
        try
        {
            body = File.ReadAllText(TemplateFileCode);
            body = body.Replace("{{code}}", code);
        }
        catch (Exception exception)
            when (exception is DirectoryNotFoundException
                || exception is FileNotFoundException
                || exception is IOException
            )
        {
            _logger.LogError(
                LogEvents.ConfigurationError,
                exception,
                "{path} could not be used",
                TemplateFileCode
            );
            throw new ApplicationException($"{TemplateFileCode} could not be used", exception);
        }

        user.AccessCode = _protector.Protect(code);
        user.AccessCodeCreatedAt = DateTime.UtcNow;
        await _accountManager.UpdateUserAsync(user);

        _emailService.Send(email, subject, body);
    }
}
