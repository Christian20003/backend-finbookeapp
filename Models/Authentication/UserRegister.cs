using System.ComponentModel.DataAnnotations;
using FinBookeAPI.Models.Authentication.Interfaces;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models the registration data received from a client
/// </summary>
public class UserRegister : IUserRegister
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Provided email address is invalid")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Username is required")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = "";
}
