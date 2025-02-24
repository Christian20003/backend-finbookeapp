using System.ComponentModel.DataAnnotations;
using FinBookeAPI.Models.Authentication.Interfaces;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models the login data received from a client.
/// </summary>
public class UserLogin : IUserLogin
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
}
