using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models a refresh token used as authenticator if a client is allowed to
/// refresh its corresponding JWT.
/// </summary>
public class RefreshToken
{
    /// <summary>
    /// The id of the token.
    /// </summary>
    [Required(ErrorMessage = "Token id is required")]
    public string Id { get; set; } = "";

    /// <summary>
    /// The refresh token as string.
    /// </summary>
    [Required(ErrorMessage = "Token value is required")]
    [MinLength(20, ErrorMessage = "The token value should have at least 20 characters")]
    public string Token { get; set; } = "";

    /// <summary>
    /// The id of the user which this token corresponds to.
    /// </summary>
    [Required(ErrorMessage = "User id is required")]
    public string UserId { get; set; } = "";

    /// <summary>
    /// The date when this token expires.
    /// </summary>
    [Required(ErrorMessage = "Expire-Date is required")]
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// The date when this token was created.
    /// </summary>
    [Required(ErrorMessage = "Create-Date is required")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// This method generates a random string with multiple characters which
    /// will be used as token value.
    /// </summary>
    public void GenerateTokenValue()
    {
        var randomNumber = new byte[64];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomNumber);
        Token = Convert.ToBase64String(randomNumber);
    }

    /// <summary>
    /// This method hashes the value of this token.
    /// </summary>
    public void HashValue()
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(Token));
        var builder = new StringBuilder();
        foreach (var elem in hash)
        {
            builder.Append(elem.ToString("x2"));
        }
        Token = builder.ToString();
    }
}
