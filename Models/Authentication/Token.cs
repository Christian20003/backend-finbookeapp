using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinBookeAPI.Models.Authentication.Interfaces;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Configuration.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models a security token for authentication (JWT).
/// </summary>
public class Token : IToken
{
    [Required(ErrorMessage = "Token values is required")]
    [MinLength(20, ErrorMessage = "The token value should have at least 20 characters")]
    public string Value { get; set; } = "";

    [Required(ErrorMessage = "Expire time is required")]
    public long Expires { get; set; }

    public void GenerateTokenValue(IOptions<IJwtSettings> settings, string identity)
    {
        // Proof if configuration is available
        var config = settings.Value;
        if (config.Audience == null || config.Issuer == null || config.Secret == null)
        {
            throw new ApplicationException(
                "JWT properties are not set. Therefore a token cannot be generated"
            );
        }

        // Set expiration time to 1 hour
        var expires = DateTime.UtcNow.AddHours(1);
        Expires = expires.Ticks;

        // Create symmetric key and token structure
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret));
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([new Claim(ClaimTypes.Name, identity)]),
            Expires = expires,
            Issuer = config.Issuer,
            Audience = config.Audience,
            SigningCredentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256Signature
            ),
        };
        // Generate token
        var token = tokenHandler.CreateToken(tokenDescriptor);
        Value = tokenHandler.WriteToken(token);
    }
}
