using FinBookeAPI.Models.Token;

namespace FinBookeAPI.Services.Token;

public partial class TokenService : ITokenService
{
    public async Task StoreAccessToken(string token)
    {
        _logger.LogDebug("Access token is added to the database: {token}", token);
        var (_, expire) = VerifyAccessToken(token);
        var tokenObj = new JwtToken { Value = token, Expires = expire };
        await _collection.Add(tokenObj);
    }
}
