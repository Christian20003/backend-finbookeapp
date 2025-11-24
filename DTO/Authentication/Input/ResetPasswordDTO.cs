using System.ComponentModel.DataAnnotations;

namespace FinBookeAPI.DTO.Authentication.Input;

/// <summary>
/// This class models a transfer object to reset password requests.
/// </summary>
public class ResetPasswordDTO
{
    [Required(ErrorMessage = "Email property is missing")]
    [EmailAddress(ErrorMessage = "Email property is not a valid email address")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Access code property is missing")]
    public string AccessCode { get; set; } = "";
}
