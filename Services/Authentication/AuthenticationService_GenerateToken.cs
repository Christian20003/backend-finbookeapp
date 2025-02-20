using System.Security.Cryptography;
using System.Text;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public UserClient GenerateToken(UserClient data)
    {
        throw new NotImplementedException();
    }

    private async Task<RefreshToken> CreateRefreshToken(UserDatabase user)
    {
        var refreshToken = new RefreshToken
        {
            Id = new Guid().ToString(),
            UserId = user.Id,
            Token = RefreshToken.GenerateToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            CreatedAt = DateTime.UtcNow,
        };
        _logger.LogDebug("Generate new refresh token: {token}", refreshToken.Token);
        user.RefreshTokenId = refreshToken.Id;
        var update = await _userManager.UpdateAsync(user);
        if (!update.Succeeded)
        {
            _logger.LogError(
                LogEvents.FAILED_UPDATE,
                "Refresh token could not be updated for user - {user}",
                user.Id
            );
            throw new AuthenticationException("Failed update of user", ErrorCodes.UPDATE_FAILED);
        }
        // Hash token for security
        using SHA256 algo = SHA256.Create();
        var creation = await _database.AddRefreshToken(
            new RefreshToken
            {
                Id = refreshToken.Id,
                UserId = refreshToken.UserId,
                Token = GetHash(algo, refreshToken.Token),
                ExpiresAt = refreshToken.ExpiresAt,
                CreatedAt = refreshToken.CreatedAt,
            }
        );
        if (creation == null)
        {
            _logger.LogError(
                LogEvents.FAILED_INSERT,
                "Refresh token could not be inserted for user - {user}",
                user.Id
            );
            throw new AuthenticationException(
                "Failed to create refresh token",
                ErrorCodes.INSERT_FAILED
            );
        }
        await _database.SaveChangesAsync();
        return creation;
    }

    private static string GetHash(HashAlgorithm algorithm, string input)
    {
        var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
        var builder = new StringBuilder();
        foreach (var elem in hash)
        {
            builder.Append(elem.ToString("x2"));
        }
        return builder.ToString();
    }
}
