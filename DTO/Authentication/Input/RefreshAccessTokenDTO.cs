using System.ComponentModel.DataAnnotations;

namespace FinBookeAPI.DTO.Authentication.Input;

/// <summary>
/// This class represents a transfer object to refresh access token requests.
/// </summary>
public class RefreshAccessTokenDTO
{
    /// <summary>
    /// The email address of the user account.
    /// </summary>
    [Required(ErrorMessage = "Email property is missing")]
    [EmailAddress(ErrorMessage = "Email property is not a valid email address")]
    public string Email { get; set; } = "";

    /// <summary>
    /// The refresh token to verify it's authorization.
    /// </summary>
    [Required(ErrorMessage = "Refresh token property is missing")]
    public string RefreshToken { get; set; } = "";
}
