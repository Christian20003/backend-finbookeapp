using FinBookeAPI.Models.Configuration;

namespace FinBookeAPI.Services.Token;

public partial class TokenService : ITokenService
{
    public (string, long) VerifyRefreshToken(string token)
    {
        _logger.LogDebug("Verify refresh token {token}", token);
        var secret = _settings.Value.RefreshTokenSecret;
        if (secret == null)
        {
            _logger.LogError(LogEvents.ConfigurationError, "Refresh token secret is null");
            throw new ApplicationException("Refresh token secret is null");
        }
        return VerifyToken(token, secret);
    }
}
