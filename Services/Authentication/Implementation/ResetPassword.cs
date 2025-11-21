using System.Net.Mail;
using FinBookeAPI.AppConfig.Redaction;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    private const string TemplateFilePwd = "../../../Templates/Email/PasswordReset.html";

    public async Task ResetPassword(string email, string code)
    {
        _logger.LogDebug("Reset password call of {email}", PrivacyGuard.Hide(_redactor, email));
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
        if (user.AccessCode == null || user.AccessCodeCreatedAt == null)
        {
            _logger.LogWarning(
                LogEvents.ResetCredentialsFailed,
                "Access code of {id} is null",
                user.Id
            );
            throw new AuthorizationException("Invalid access code");
        }
        var expire = user.AccessCodeCreatedAt.Value.AddMinutes(10);
        if (DateTime.UtcNow.Ticks - expire.Ticks > 0)
        {
            _logger.LogWarning(
                LogEvents.AuthorizationFailed,
                "Access code of {id} expired",
                user.Id
            );
            throw new AuthorizationException("Access code expired");
        }
        if (user.AccessCode != _protector.Protect(code))
        {
            _logger.LogWarning(
                LogEvents.AuthorizationFailed,
                "Access code of {id} is invalid",
                user.Id
            );
            throw new AuthorizationException("Invalid access code");
        }
        var password = _securityUtilityService.GeneratePassword(20);
        var subject = "Requested password reset";
        string? body;
        try
        {
            body = File.ReadAllText(TemplateFilePwd);
            body = body.Replace("{{password}}", password);
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
                TemplateFilePwd
            );
            throw new ApplicationException($"{TemplateFilePwd} could not be used", exception);
        }

        user.AccessCode = null;
        user.AccessCodeCreatedAt = null;
        await _accountManager.UpdateUserAsync(user);
        await _accountManager.SetPasswordAsync(user, password);

        _emailService.Send(email, subject, body);
        _logger.LogInformation(
            LogEvents.ResetCredentialsSuccess,
            "Successfull password reset for {id}",
            user.Id
        );
    }
}
