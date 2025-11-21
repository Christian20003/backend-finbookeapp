using FinBookeAPI.AppConfig.Redaction;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public async Task<User> Login(string email, string password)
    {
        _logger.LogDebug("Login call of {email}", PrivacyGuard.Hide(_redactor, email));
        if (!VerifyEmail(email))
        {
            _logger.LogWarning(
                LogEvents.AuthenticationFailed,
                "{email} is not a valid email-address",
                PrivacyGuard.Hide(_redactor, email)
            );
            throw new ArgumentException($"{email} is not a valid email-address", nameof(email));
        }
        var user = await VerifyUserAccount(email);
        if (user.IsRevoked)
        {
            _logger.LogWarning(LogEvents.AuthenticationFailed, "User account has been revoked");
            throw new ResourceLockedException($"User account of {email} has been revoked");
        }
        await VerifyPassword(user, password);

        var accessToken = _tokenService.GenerateAccessToken(user.Id);
        var refreshToken = _tokenService.GenerateRefreshToken(user.Id);

        _logger.LogInformation(
            LogEvents.AuthenticationSuccess,
            "{email} has logged in successfully",
            PrivacyGuard.Hide(_redactor, email)
        );

        return new User
        {
            Id = Guid.Parse(user.Id),
            Name = _protector.Unprotect(user.UserName!),
            Email = _protector.UnprotectEmail(user.Email!),
            ImagePath = user.ImagePath,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
    }
}
