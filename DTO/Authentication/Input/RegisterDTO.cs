using System.ComponentModel.DataAnnotations;

namespace FinBookeAPI.DTO.Authentication.Input;

/// <summary>
/// This class models a transfer object for registration requests.
/// </summary>
public class RegisterDTO
{
    [Required(ErrorMessage = "Email porperty is missing")]
    [EmailAddress(ErrorMessage = "Email property is not a valid email address")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Name property is missing")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Password property is missing")]
    public string Password { get; set; } = "";
}
