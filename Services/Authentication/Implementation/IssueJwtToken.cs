using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Token;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public async Task<JwtToken> IssueJwtToken(string refreshToken)
    {
        _logger.LogDebug("Generate a new access token.");
        var (id, _) = _tokenService.VerifyRefreshToken(refreshToken);
        if (await _tokenService.TokenExists(refreshToken))
        {
            _logger.LogError(
                LogEvents.AuthenticationFailed,
                "Used refresh token is blacklisted: {token}",
                refreshToken
            );
            throw new ArgumentException("Provided refresh token is revoked", nameof(refreshToken));
        }
        var result = _tokenService.GenerateAccessToken(id);
        _logger.LogInformation(
            LogEvents.AuthenticationSuccess,
            "Generated new access token successfully"
        );
        return result;
    }
}
