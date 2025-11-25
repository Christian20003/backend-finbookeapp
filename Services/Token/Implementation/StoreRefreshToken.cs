using FinBookeAPI.Models.Token;

namespace FinBookeAPI.Services.Token;

public partial class TokenService : ITokenService
{
    public async Task StoreRefreshToken(string token)
    {
        _logger.LogDebug("Refresh token is added to the database: {token}", token);
        var (_, expire) = VerifyRefreshToken(token);
        var tokenObj = new JwtToken { Value = token, Expires = expire };
        await _collection.Add(tokenObj);
    }
}
