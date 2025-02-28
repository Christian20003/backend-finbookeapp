using System.ComponentModel.DataAnnotations;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models a session object which stores the authentication tokens (JWT and refresh token).
/// </summary>
public class Session
{
    /// <summary>
    /// A JWT token for authentication.
    /// </summary>
    [Required(ErrorMessage = "Token is required")]
    public Token Token { get; set; } = new Token();

    /// <summary>
    /// A refresh token to be able of refreshing the JWT.
    /// </summary>
    [Required(ErrorMessage = "Refresh token is required")]
    public RefreshToken RefreshToken { get; set; } = new RefreshToken();
}
