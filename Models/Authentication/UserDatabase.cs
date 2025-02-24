using System.ComponentModel.DataAnnotations;
using FinBookeAPI.Models.Authentication.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models a user account of this application storing all profile imformation.
/// </summary>
public class UserDatabase : IdentityUser, IUserDatabase
{
    // Properties name, email and password are already implemented in base class
    [Required]
    public string ImagePath { get; set; } = "";

    [Required]
    public string RefreshTokenId { get; set; } = "";

    [Required]
    public bool IsRevoked { get; set; } = false;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime RevokedAt { get; set; }
}
