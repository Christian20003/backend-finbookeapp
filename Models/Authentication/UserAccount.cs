using Microsoft.AspNetCore.Identity;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models a user account of this application storing all profile imformation.
/// </summary>
public class UserAccount : IdentityUser
{
    // Properties name, email and password are already implemented in base class

    public string ImagePath { get; set; } = "";
    public string? AccessCode { get; set; }
    public DateTime? AccessCodeCreatedAt { get; set; }
    public bool IsRevoked { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedAt { get; set; }
}
