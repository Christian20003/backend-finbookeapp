using System.ComponentModel.DataAnnotations;
using FinBookeAPI.Models.Authentication.Interfaces;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models a reset password request with all necessary information.
/// </summary>
public class UserResetRequest : IUserResetRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Provided email address is invalid")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Security-code is required")]
    public string? Code { get; set; } = null;
}
