using System.ComponentModel.DataAnnotations;

namespace FinBookeAPI.DTO.Authentication.Input;

/// <summary>
/// This class represents a transfer object for forget passsword requests.
/// </summary>
public class ForgotPwdDTO
{
    /// <summary>
    /// The email address of the user account.
    /// </summary>
    [Required(ErrorMessage = "Email property is missing")]
    [EmailAddress(ErrorMessage = "Email property is not a valid email address")]
    public string Email { get; set; } = "";
}
