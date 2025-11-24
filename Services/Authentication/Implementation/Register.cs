using FinBookeAPI.AppConfig.Redaction;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public async Task<User> Register(string email, string userName, string password)
    {
        _logger.LogDebug("Register call of {email}", PrivacyGuard.Hide(_redactor, email));

        if (userName == string.Empty)
        {
            _logger.LogWarning(
                LogEvents.AuthenticationFailed,
                "User name of {email} is empty",
                PrivacyGuard.Hide(_redactor, email)
            );
            throw new ArgumentException("User name is empty", nameof(userName));
        }
        if (!VerifyEmail(email))
        {
            _logger.LogWarning(
                LogEvents.AuthenticationFailed,
                "{email} is not a valid email address",
                PrivacyGuard.Hide(_redactor, email)
            );
            throw new ArgumentException($"{email} is not a valid email-address", nameof(email));
        }
        var newUser = new UserAccount
        {
            UserName = _protector.Protect(userName),
            Email = _protector.ProtectEmail(email),
        };
        var result = await _accountManager.CreateUserAsync(newUser, password);
        if (!result.Succeeded)
        {
            _logger.LogWarning(LogEvents.AuthenticationFailed, "User account conditions violated");
            throw new IdentityResultException(result.Errors, "User account conditions violated");
        }

        newUser = await VerifyUserAccount(email);
        var refreshToken = _tokenService.GenerateRefreshToken(newUser.Id);
        var jwtToken = _tokenService.GenerateAccessToken(newUser.Id);

        _logger.LogInformation(
            LogEvents.AuthenticationSuccess,
            "Successfully created new user account: {Id}",
            newUser.Id
        );
        return new User
        {
            Id = Guid.Parse(newUser.Id),
            Name = _protector.Unprotect(newUser.UserName!),
            Email = _protector.UnprotectEmail(newUser.Email!),
            ImagePath = newUser.ImagePath,
            AccessToken = jwtToken,
            RefreshToken = refreshToken,
        };
    }
}
