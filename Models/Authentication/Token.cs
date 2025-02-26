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
/// This class models a security token for authentication.
/// </summary>
public class Token : IToken
{
    // The token value
    public string Value { get; }

    // The time after this token expires
    public long Expires { get; }

    private readonly IOptions<IJwtSettings> _settings;

    public Token(string userName, IOptions<IJwtSettings> settings)
    {
        _settings = settings;
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
            Subject = new ClaimsIdentity([new Claim(ClaimTypes.Name, userName)]),
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

    public Token(string token, long expires, IOptions<JwTSettings> settings)
    {
        _settings = settings;
        Value = token;
        Expires = expires;
    }

    public Token()
    {
        Value = "";
        Expires = 0;
        _settings = Options.Create(new JwTSettings());
    }

    public string GetSubject()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        if (tokenHandler.ReadToken(Value) is not JwtSecurityToken token)
        {
            throw new ApplicationException("Invalid JWT token");
        }
        return token.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
    }
}
