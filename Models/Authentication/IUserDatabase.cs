namespace FinBookeAPI.Models.Authentication;

public interface IUserDatabase
{
    // Ths id of the user
    public string Id { get; set; }

    // Name of the user
    public string? UserName { get; set; }

    // Email of the user
    public string? Email { get; set; }

    // Hash of the password
    public string? PasswordHash { get; set; }

    // This string store the path to the profile image
    public string ImagePath { get; set; }

    // Id from the refresh token (only available if logged in)
    public string RefreshTokenId { get; set; }

    // If the account is deactivated (delete request)
    public bool IsRevoked { get; set; }

    // Date when this account was created
    public DateTime CreatedAt { get; set; }

    // Date when this account was deactivated
    public DateTime RevokedAt { get; set; }
}
