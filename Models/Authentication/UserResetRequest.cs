using System.ComponentModel.DataAnnotations;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models a reset password request with all necessary information.
/// </summary>
public class UserResetRequest
{
    /// <summary>
    /// The email of the reset password request.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Provided email address is invalid")]
    public string Email { get; set; } = "";

    /// <summary>
    /// The generated code to authenticate for password reset.
    /// </summary>
    [Required(ErrorMessage = "Security-code is required")]
    public string? Code { get; set; } = null;
}
