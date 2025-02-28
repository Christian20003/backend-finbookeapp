using System.ComponentModel.DataAnnotations;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models the registration data received from a client
/// </summary>
public class UserRegister
{
    /// <summary>
    /// The email address of the register request.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Provided email address is invalid")]
    public string Email { get; set; } = "";

    /// <summary>
    /// The user name of the register request.
    /// </summary>
    [Required(ErrorMessage = "Username is required")]
    public string Name { get; set; } = "";

    /// <summary>
    /// The password of the register request.
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = "";
}
