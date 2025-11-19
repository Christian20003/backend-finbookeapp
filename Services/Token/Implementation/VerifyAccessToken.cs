using FinBookeAPI.Models.Configuration;

namespace FinBookeAPI.Services.Token;

public partial class TokenService : ITokenService
{
    public (string, long) VerifyAccessToken(string token)
    {
        _logger.LogDebug("Verify access token {token}", token);
        var secret = _settings.Value.AccessTokenSecret;
        if (secret == null)
        {
            _logger.LogError(LogEvents.ConfigurationError, "Access token secret is null");
            throw new ApplicationException("Access token secret is null");
        }
        return VerifyToken(token, secret);
    }
}
