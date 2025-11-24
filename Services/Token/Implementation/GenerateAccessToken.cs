using System.Security.Claims;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Token;

namespace FinBookeAPI.Services.Token;

public partial class TokenService : ITokenService
{
    public JwtToken GenerateAccessToken(string userId)
    {
        _logger.LogDebug("Generate a new access token for {userId}", userId);
        var lifetime = _settings.Value.AccessTokenExpireM;
        var secret = _settings.Value.AccessTokenSecret;
        if (lifetime <= 0)
        {
            _logger.LogError(
                LogEvents.ConfigurationError,
                "Invalid expiration time for the access token: {time}",
                lifetime
            );
            throw new ApplicationException(
                "Expiration time of access tokens must be larger than zero"
            );
        }
        if (secret == null)
        {
            _logger.LogError(LogEvents.ConfigurationError, "AccessTokenSecret is null");
            throw new ApplicationException("AccessTokenSecret is null");
        }
        var expire = DateTime.UtcNow.AddMinutes(lifetime);
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, userId) };
        var token = GenerateToken(claims, secret, expire);
        return new JwtToken { Value = token, Expires = expire.Ticks };
    }
}
