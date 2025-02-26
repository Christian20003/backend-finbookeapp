namespace FinBookeAPI.Models.Authentication.Interfaces;

public interface IUserDatabase
{
    /// <summary>
    /// Ths id of the user.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The name of the user.
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// The email address of the user.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// The hash of the password.
    /// </summary>
    public string? PasswordHash { get; set; }

    /// <summary>
    /// The path to the profile image.
    /// </summary>
    public string ImagePath { get; set; }

    /// <summary>
    /// The id of the refresh token (only available if logged in).
    /// </summary>
    public string RefreshTokenId { get; set; }

    /// <summary>
    /// If the account is deactivated (delete request).
    /// </summary>
    public bool IsRevoked { get; set; }

    /// <summary>
    /// The date when this account was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The date when this account was deactivated.
    /// </summary>
    public DateTime RevokedAt { get; set; }
}
