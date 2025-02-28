using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinBookeAPI.Models.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models a security token for authentication (JWT).
/// </summary>
public class Token
{
    /// <summary>
    ///  The token value.
    /// </summary>
    [Required(ErrorMessage = "Token values is required")]
    [MinLength(20, ErrorMessage = "The token value should have at least 20 characters")]
    public string Value { get; set; } = "";

    /// <summary>
    /// The date after this token expires in milliseconds.
    /// </summary>
    [Required(ErrorMessage = "Expire time is required")]
    public long Expires { get; set; }

    /// <summary>
    /// This method generates a new token value with the provided configuration
    /// in the settings object. If any configuration options is missing an
    /// <c>ApplicationException</c> will be thrown
    /// </summary>
    /// <param name="settings">
    /// The options containing all necessary configuration data from
    /// <c>appsettings.json</c>.
    /// </param>
    /// <param name="identity">
    /// The main identity value for the subject property in a JWT.
    /// </param>
    /// <exception cref="ApplicationException"></exception>
    public void GenerateTokenValue(IOptions<JwtSettings> settings, string identity)
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
