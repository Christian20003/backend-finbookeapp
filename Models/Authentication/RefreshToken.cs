using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models a refresh token used as authenticator if a client is allowed to
/// refresh its corresponding JWT.
/// </summary>
public class RefreshToken
{
    [Required]
    public string Id { get; set; } = "";

    // The refresh token as string
    [Required]
    public string Token { get; set; } = "";

    // The id of the user which this token corresponds to
    [Required]
    public string UserId { get; set; } = "";

    // Date when this token expires
    [Required]
    public DateTime ExpiresAt { get; set; }

    // Date when this token was created
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// This method generates a random string token by using the <c>RandomNumberGenerator</c>.
    /// </summary>
    /// <returns>The generated token as string.</returns>
    public static string GenerateToken()
    {
        var randomNumber = new byte[64];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
