using FinBookeAPI.Models.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public async Task Logout(string accessToken, string refreshToken)
    {
        _logger.LogDebug("Logout call");
        try
        {
            await _tokenService.StoreAccessToken(accessToken);
        }
        catch (SecurityTokenExpiredException exception)
        {
            _logger.LogInformation(
                LogEvents.OperationIgnored,
                exception,
                "Access token has already expired: {token}",
                accessToken
            );
        }
        try
        {
            await _tokenService.StoreRefreshToken(refreshToken);
        }
        catch (SecurityTokenExpiredException exception)
        {
            _logger.LogInformation(
                LogEvents.OperationIgnored,
                exception,
                "Refresh token has already expired: {token}",
                refreshToken
            );
        }
        _logger.LogInformation(LogEvents.AuthenticationSuccess, "Successful logout");
    }
}
