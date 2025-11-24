using System.Security.Authentication;
using FinBookeAPI.AppConfig.Redaction;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    /// <summary>
    /// This method proofs if the provided password corresponds to the user account and is valid.
    /// </summary>
    /// <param name="user">
    /// The user account.
    /// </param>
    /// <param name="password">
    /// The password received from a client.
    /// </param>
    /// <exception cref="InvalidCredentialException">
    /// If the provided password does not match with the stored one.
    /// </exception>
    /// <exception cref="ResourceLockedException">
    /// If user account is locked for further login attempts.
    /// </exception>
    private async Task VerifyPassword(UserAccount user, string password)
    {
        _logger.LogDebug(
            "Verify user password of {email}",
            PrivacyGuard.Hide(_redactor, user.Email)
        );
        var check = await _signInManager.CheckPasswordSignInAsync(
            user,
            password,
            lockoutOnFailure: true
        );
        if (check == SignInResult.Failed)
        {
            _logger.LogWarning(
                LogEvents.AuthenticationFailed,
                "Provided password of user {id} is not valid",
                user.Id
            );
            throw new InvalidCredentialException("Invalid password");
        }
        else if (check == SignInResult.LockedOut)
        {
            _logger.LogWarning(
                LogEvents.AuthenticationFailed,
                "The user account of {id} is temporarily locked.",
                user.Id
            );
            throw new ResourceLockedException(
                "The user account has been temporarily locked for login."
            );
        }
    }
}
