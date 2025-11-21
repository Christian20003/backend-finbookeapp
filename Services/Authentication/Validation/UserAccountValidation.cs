using System.Security.Authentication;
using FinBookeAPI.AppConfig.Redaction;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    /// <summary>
    /// This method proofs if any user account exist in the authentication database with the provided email address.
    /// </summary>
    /// <param name="email">
    /// The email address of the user account.
    /// </param>
    /// <returns>
    /// The user account that corresponds to the given email address.
    /// </returns>
    /// <exception cref="InvalidCredentialException">
    /// If the email address does not correspond to any user account.
    /// </exception>
    private async Task<UserAccount> VerifyUserAccount(string email)
    {
        _logger.LogDebug("Verify user account of {email}", PrivacyGuard.Hide(_redactor, email));
        var accounts = _accountManager.GetUsersAsync();
        var user = await accounts.FirstOrDefaultAsync(account =>
            account.Email == _protector.ProtectEmail(email)
        );
        // Proof if user exist
        if (user == null)
        {
            _logger.LogWarning(
                LogEvents.AuthenticationFailed,
                "{email} does not have a user account",
                PrivacyGuard.Hide(_redactor, email)
            );
            throw new InvalidCredentialException($"{email} does not have a user account");
        }
        return user;
    }
}
