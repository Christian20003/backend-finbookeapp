namespace FinBookeAPI.Services.Token;

public partial class TokenService : ITokenService
{
    public async Task<bool> TokenExists(string token)
    {
        _logger.LogDebug("Proof if token exists in database: {token}", token);
        return await _collection.Contains(token);
    }
}
