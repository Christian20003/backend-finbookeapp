using System.Security.Claims;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Token;

namespace FinBookeAPI.Services.Token;

public partial class TokenService : ITokenService
{
    public JwtToken GenerateRefreshToken(string userId)
    {
        _logger.LogDebug("Generate new refresh token for {userId}", userId);
        var lifetime = _settings.Value.RefreshTokenExpireD;
        var secret = _settings.Value.RefreshTokenSecret;
        if (lifetime <= 0)
        {
            _logger.LogError(
                LogEvents.ConfigurationError,
                "Invalid expiration time for the refresh token: {time}",
                lifetime
            );
            throw new ApplicationException(
                "Expiration time of refresh tokens must be larger than zero"
            );
        }
        if (secret == null)
        {
            _logger.LogError(LogEvents.ConfigurationError, "AccessTokenSecret is null");
            throw new ApplicationException("AccessTokenSecret is null");
        }
        var expires = DateTime.UtcNow.AddDays(lifetime);
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, userId) };
        var token = GenerateToken(claims, secret, expires);
        return new JwtToken { Value = token, Expires = expires.Ticks };
    }
}
