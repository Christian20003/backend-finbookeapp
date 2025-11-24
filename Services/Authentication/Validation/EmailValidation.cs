using System.ComponentModel.DataAnnotations;
using FinBookeAPI.AppConfig.Redaction;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    /// <summary>
    /// This method proofs if the provided email is a valid email address.
    /// </summary>
    /// <param name="email">
    /// The email value that should be verified.
    /// </param>
    /// <returns>
    /// <c>True</c> if the provided email is valid, otherwise <c>false</c>.
    /// </returns>
    private bool VerifyEmail(string email)
    {
        // TODO: User needs to verify his address
        _logger.LogDebug(
            "Verify given email address: {email}",
            PrivacyGuard.Hide(_redactor, email)
        );
        var emailValidator = new EmailAddressAttribute();
        if (!emailValidator.IsValid(email))
        {
            return false;
        }
        return true;
    }
}
