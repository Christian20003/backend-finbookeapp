using System.ComponentModel.DataAnnotations;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models the login data received from a client.
/// </summary>
public class UserLogin
{
    /// <summary>
    /// The email of the login request.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Provided email address is invalid")]
    public string Email { get; set; } = "";

    /// <summary>
    /// The password of the login request.
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = "";
}
