using System.ComponentModel.DataAnnotations;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models the registration data received from a client
/// </summary>
public class UserRegister
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    public string Name { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
}
