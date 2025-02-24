using System.ComponentModel.DataAnnotations;
using FinBookeAPI.Models.Authentication.Interfaces;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models a authentication request including a refresh token to
/// authenticate and an email to a valid user account.
/// </summary>
public class UserTokenRequest : IUserTokenRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    public IRefreshToken Token { get; set; } = new RefreshToken();
}
