using System.ComponentModel.DataAnnotations;
using FinBookeAPI.Models.Authentication.Interfaces;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models a session object which stores the authentication tokens (JWT and refresh token).
/// </summary>
public class Session : Interfaces.ISession
{
    [Required(ErrorMessage = "Token is required")]
    public IToken Token { get; set; } = new Token();

    [Required(ErrorMessage = "Refresh token is required")]
    public IRefreshToken RefreshToken { get; set; } = new RefreshToken();
}
