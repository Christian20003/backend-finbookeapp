using System.Security.Claims;
using FinBookeAPI.Models.Token;

namespace FinBookeAPI.Services.Token;

public partial class TokenService : ITokenService
{
    public JwtToken GenerateAccessToken(string userId)
    {
        _logger.LogDebug("Generate a new access token for {userId}", userId);
        if (_settings.Value.AccessTokenExpireM <= 0)
        {
            throw new ApplicationException(
                "Expiration time of access tokens must be larger than zero"
            );
        }
        var secret =
            _settings.Value.AccessTokenSecret
            ?? throw new ApplicationException("Missing configuration data to generate tokens.");
        var expire = DateTime.UtcNow.AddMinutes(_settings.Value.AccessTokenExpireM);
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, userId) };
        var token = GenerateToken(claims, secret, expire);
        return new JwtToken { Value = token, Expires = expire.Ticks };
    }
}
