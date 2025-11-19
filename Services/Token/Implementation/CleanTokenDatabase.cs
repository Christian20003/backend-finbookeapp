namespace FinBookeAPI.Services.Token;

public partial class TokenService : ITokenService
{
    public async Task CleanTokenDatabase()
    {
        _logger.LogDebug("Clean token database");
        await _collection.Delete();
    }
}
