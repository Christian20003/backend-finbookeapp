using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models a user account of this application storing all profile imformation.
/// </summary>
public class UserDatabase : IdentityUser
{
    // Properties name, email and password are already implemented in base class
    // This string store the path to the profile image
    [Required]
    public string ImagePath { get; set; } = "";

    // Id from the refresh token (only available if logged in)
    [Required]
    public string RefreshTokenId { get; set; } = "";

    // If the account is deactivated (delete request)
    [Required]
    public bool IsRevoked { get; set; } = false;

    // Date when this account was created
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Date when this account was deactivated
    [Required]
    public DateTime RevokedAt { get; set; }
}
