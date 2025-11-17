using System.Security.Claims;
using FinBookeAPI.Models.Token;

namespace FinBookeAPI.Services.Token;

public partial class TokenService : ITokenService
{
    public JwtToken GenerateRefreshToken(string userId)
    {
        _logger.LogDebug("Generate new refresh token for {userId}", userId);
        if (_settings.Value.RefreshTokenExpireD <= 0)
        {
            throw new ApplicationException(
                "Expiration time of refresh tokens must be larger than zero"
            );
        }
        var secret =
            _settings.Value.RefreshTokenSecret
            ?? throw new ApplicationException("Missing configuration data to generate tokens.");
        var expires = DateTime.UtcNow.AddDays(_settings.Value.RefreshTokenExpireD);
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, userId) };
        var token = GenerateToken(claims, secret, expires);
        return new JwtToken { Value = token, Expires = expires.Ticks };
    }
}
