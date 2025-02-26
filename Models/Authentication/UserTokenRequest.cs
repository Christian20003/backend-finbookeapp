using System.ComponentModel.DataAnnotations;
using FinBookeAPI.Models.Authentication.Interfaces;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models a authentication request including a refresh token to
/// authenticate and an email to a valid user account.
/// </summary>
public class UserTokenRequest : IUserTokenRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Provided email address is invalid")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Refresh token is required")]
    public IRefreshToken Token { get; set; } = new RefreshToken();
}
