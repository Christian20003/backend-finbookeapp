using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinBookeAPI.Models.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FinBookeAPI.Services.Token;

public partial class TokenService : ITokenService
{
    /// <summary>
    /// This method verifies a JWT token
    /// </summary>
    /// <param name="token">
    /// The token that should be verified
    /// </param>
    /// <param name="secret">
    /// The secret to generate a symmetric key.
    /// </param>
    /// <returns>
    /// The user id and the time where this token expires.
    /// </returns>
    /// <exception cref="ApplicationException">
    /// If required configuration data is null or the secret has less than 16 bytes.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// If the provided token is null or empty.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If the provided token exceeds the maximum length.
    /// </exception>
    /// <exception cref="SecurityTokenMalformedException">
    /// If the token does not fulfill the required structure.
    /// </exception>
    /// <exception cref="SecurityTokenEncryptionKeyNotFoundException">
    /// If the 'kid' header claim is not null AND decryption fails.
    /// </exception>
    /// <exception cref="SecurityTokenException">
    /// If the 'enc' header claim is null or empty.
    /// </exception>
    /// <exception cref="SecurityTokenExpiredException">
    /// If the token has expired.
    /// </exception>
    /// <exception cref="SecurityTokenInvalidSignatureException">
    /// If the signature is not valid.
    /// </exception>
    /// <exception cref="SecurityTokenInvalidIssuerException">
    /// If the 'issuer' property in the token is invalid.
    /// </exception>
    /// <exception cref="SecurityTokenInvalidAudienceException">
    /// The 'audience' property in the token is invalid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// If the user id is not inside the token.
    /// </exception>
    private (string, long) VerifyToken(string token, string secret)
    {
        _logger.LogDebug("Verify token {token}", token);
        var audience = _settings.Value.Audience;
        var issuer = _settings.Value.Issuer;
        var bytes = Encoding.UTF8.GetBytes(secret);

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
        if (bytes.Length < 16)
        {
            _logger.LogError(
                LogEvents.ConfigurationError,
                "Given secret is too small to generated symmetric key"
            );
            throw new ApplicationException("Given secret is too small to generated symmetric key");
        }
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParam = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(bytes),
            ClockSkew = TimeSpan.Zero,
        };
        var principles = tokenHandler.ValidateToken(token, validationParam, out _);
        var id = principles.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
        var expires = principles.Claims.First(claim => claim.Type == "exp");
        var expDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expires.Value)).UtcDateTime;
        return (id.Value, expDateTime.Ticks);
    }
}
