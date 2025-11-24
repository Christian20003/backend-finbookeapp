using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinBookeAPI.Models.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FinBookeAPI.Services.Token;

public partial class TokenService : ITokenService
{
    /// <summary>
    /// This method generates a new token for authentication.
    /// </summary>
    /// <param name="claims">
    /// A list of <see cref="Claim"/> objects that should be included inside the token.
    /// </param>
    /// <param name="secret">
    /// A secret to generate a symmetric key for a signature.
    /// </param>
    /// <param name="expires">
    /// The date where this token should expire.
    /// </param>
    /// <returns>
    /// The generates token.
    /// </returns>
    /// <exception cref="ApplicationException">
    /// If necessary configuration data is null or the provided secret has less than 16 byte
    /// </exception>
    private string GenerateToken(IEnumerable<Claim> claims, string secret, DateTime expires)
    {
        _logger.LogDebug("Generate a new token");
        var audience = _settings.Value.Audience;
        var issuer = _settings.Value.Issuer;
        if (audience == null)
        {
            _logger.LogError(LogEvents.ConfigurationError, "Audience configuration is null");
            throw new ApplicationException("Audience configuration is null");
        }
        if (issuer == null)
        {
            _logger.LogError(LogEvents.ConfigurationError, "Issuer configuration is null");
            throw new ApplicationException("Issuer configuration is null");
        }

        var bytes = Encoding.UTF8.GetBytes(secret);
        if (bytes.Length < 16)
        {
            _logger.LogError(
                LogEvents.ConfigurationError,
                "Secret has less than 16 bytes: {length}",
                bytes.Length
            );
            throw new ApplicationException("Given secret is too small to generated symmetric key");
        }
        var key = new SymmetricSecurityKey(bytes);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256Signature
            ),
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
