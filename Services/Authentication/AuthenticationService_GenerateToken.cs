using System.Security.Cryptography;
using System.Text;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Authentication.Interfaces;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public Task<IUserClient> GenerateToken(IUserTokenRequest request)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// This method creates a new <c>RefreshToken</c>. Thereby the provided <c>user</c> instance will be updated
    /// as well as the new token added to the database (persistently stored).
    /// </summary>
    /// <param name="user">
    /// The user instance from the authentication database.
    /// </param>
    /// <returns>
    /// The new created token.
    /// </returns>
    /// <exception cref="AuthenticationException">
    /// If any database operation fail.
    /// </exception>
    private async Task<IRefreshToken> CreateRefreshToken(UserDatabase user)
    {
        // Generate new token
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
        try
        {
            // Update user object in database
            await UpdateUser(user);

            // Add new token to database
            refreshToken.HashValue();
            var creation = await _database.AddRefreshToken(refreshToken);
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
        catch (Exception exception)
            when (exception is OperationCanceledException
                || exception is DbUpdateException
                || exception is DbUpdateConcurrencyException
            )
        {
            _logger.LogError(
                LogEvents.FAILED_OPERATION,
                exception,
                "Database operation has been canceled"
            );
            throw new AuthenticationException(
                "Database operation has been canceled",
                ErrorCodes.OPERATION_CANCELED,
                exception
            );
        }
    }
}
