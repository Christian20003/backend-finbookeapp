using System.ComponentModel.DataAnnotations;
using FinBookeAPI.Models.Authentication.Interfaces;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models the registration data received from a client
/// </summary>
public class UserRegister : IUserRegister
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    public string Name { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
}
