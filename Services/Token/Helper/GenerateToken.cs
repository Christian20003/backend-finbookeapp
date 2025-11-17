using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        var config = _settings.Value;
        if (config.Audience == null || config.Issuer == null)
        {
            throw new ApplicationException("Missing configuration data to generate tokens.");
        }

        var bytes = Encoding.UTF8.GetBytes(secret);
        if (bytes.Length < 16)
        {
            throw new ApplicationException("Given secret is too small to generated symmetric key");
        }
        var key = new SymmetricSecurityKey(bytes);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            Issuer = config.Issuer,
            Audience = config.Audience,
            SigningCredentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256Signature
            ),
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
