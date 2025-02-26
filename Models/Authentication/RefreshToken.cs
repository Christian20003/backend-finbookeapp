using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using FinBookeAPI.Models.Authentication.Interfaces;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models a refresh token used as authenticator if a client is allowed to
/// refresh its corresponding JWT.
/// </summary>
public class RefreshToken : IRefreshToken
{
    [Required(ErrorMessage = "Token id is required")]
    public string Id { get; set; } = "";

    [Required(ErrorMessage = "Token value is required")]
    [MinLength(20, ErrorMessage = "The token value should have at least 20 characters")]
    public string Token { get; set; } = "";

    [Required(ErrorMessage = "User id is required")]
    public string UserId { get; set; } = "";

    [Required(ErrorMessage = "Expire-Date is required")]
    public DateTime ExpiresAt { get; set; }

    [Required(ErrorMessage = "Create-Date is required")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public void GenerateTokenValue()
    {
        var randomNumber = new byte[64];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomNumber);
        Token = Convert.ToBase64String(randomNumber);
    }

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
