using System.ComponentModel.DataAnnotations;
using FinBookeAPI.Models.Authentication;

namespace FinBookeAPI.DTO.Authentication;

/// <summary>
/// The class <c>ReauthenticateDTO</c> represents a transfer object to be able of generating
/// a new JWT.
/// </summary>
public class ReauthenticateDTO
{
    /// <summary>
    /// The email address of the user account.
    /// </summary>
    [Required(ErrorMessage = "Email property is missing")]
    [EmailAddress(ErrorMessage = "Email property should be a valid email address")]
    public string Email { get; set; } = "";

    /// <summary>
    /// The refresh token to verify it's authorization.
    /// </summary>
    [Required(ErrorMessage = "Refresh token property is missing")]
    public string RefreshToken { get; set; } = "";

    /// <summary>
    /// This method converts the current <c>ReauthenticateDTO</c> instance into an object that
    /// can be processed by the provided <c>AuthenticationService</c>.
    /// </summary>
    /// <returns>
    /// The object which can be used in the <c>AuthenticationService</c>.
    /// </returns>
    public UserTokenRequest GetUserTokenRequest()
    {
        var token = new RefreshToken { Token = RefreshToken };
        return new UserTokenRequest { Email = Email, Token = token };
    }
}
