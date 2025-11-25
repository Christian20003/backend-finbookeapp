using System.ComponentModel.DataAnnotations;

namespace FinBookeAPI.DTO.Authentication.Input;

/// <summary>
/// This class models a transfer object for login requests.
/// </summary>
public class LoginDTO
{
    [Required(ErrorMessage = "Email property is missing")]
    [EmailAddress(ErrorMessage = "Email property is not a valid email address")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Password property is missing")]
    public string Password { get; set; } = "";
}
