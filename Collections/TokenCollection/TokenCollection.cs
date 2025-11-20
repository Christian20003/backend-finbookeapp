using FinBookeAPI.AppConfig;
using FinBookeAPI.Models.Token;
using Microsoft.EntityFrameworkCore;

namespace FinBookeAPI.Collections.TokenCollection;

public class TokenCollection(AuthDbContext context) : ITokenCollection
{
    private readonly AuthDbContext _database = context;

    public async Task Add(JwtToken token)
    {
        await _database.TokenCollection.AddAsync(token);
        await _database.SaveChangesAsync();
    }

    public async Task<bool> Contains(string token)
    {
        var result = await _database.TokenCollection.FirstOrDefaultAsync(entity =>
            entity.Value == token
        );
        return result == null;
    }

    public async Task Delete()
    {
        var elements = _database
            .TokenCollection.Where(entity => DateTime.UtcNow.Ticks > entity.Expires)
            .ToList();
        _database.TokenCollection.RemoveRange(elements);
        await _database.SaveChangesAsync();
    }
}
