using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models a user account of this application storing all profile imformation.
/// </summary>
public class UserDatabase : IdentityUser
{
    // Properties name, email and password are already implemented in base class

    /// <summary>
    /// The path to the profile image.
    /// </summary>
    [Required(ErrorMessage = "ImagePath is required")]
    public string ImagePath { get; set; } = "";

    /// <summary>
    /// The id of the refresh token (only available if logged in).
    /// </summary>
    [Required(ErrorMessage = "Refresh token id is required")]
    public string RefreshTokenId { get; set; } = "";

    /// <summary>
    /// The security code creates by the server to allow the user
    /// to reset his password.
    /// </summary>
    /* [Required(ErrorMessage = "Security code is required")] */
    public string? SecurityCode { get; set; }

    /// <summary>
    /// This timestamp respresents the date when the security expires.
    /// </summary>
    /* [Required(ErrorMessage = "Create-date of security code is required")] */
    public DateTime? SecurityCodeCreatedAt { get; set; }

    /// <summary>
    /// If the account is deactivated (delete request).
    /// </summary>
    [Required(ErrorMessage = "IsRevoked bool is required")]
    public bool IsRevoked { get; set; } = false;

    /// <summary>
    /// The date when this account was created.
    /// </summary>
    [Required(ErrorMessage = "Create-Date is required")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The date when this account was deactivated.
    /// </summary>
    [Required(ErrorMessage = "Revoked-Date is required")]
    public DateTime RevokedAt { get; set; }
}
