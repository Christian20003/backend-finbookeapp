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
    [Required(ErrorMessage = "ImagePath is required")]
    public string ImagePath { get; set; } = "";

    [Required(ErrorMessage = "Refresh token id is required")]
    public string RefreshTokenId { get; set; } = "";

    [Required(ErrorMessage = "IsRevoked bool is required")]
    public bool IsRevoked { get; set; } = false;

    [Required(ErrorMessage = "Create-Date is required")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "Revoked-Date is required")]
    public DateTime RevokedAt { get; set; }
}
