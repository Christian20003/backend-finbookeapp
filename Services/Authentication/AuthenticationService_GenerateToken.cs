using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public async Task<UserClient> GenerateToken(UserTokenRequest request)
    {
        _logger.LogDebug("Generate a new JWT for {user}", request.Email);
        var user = await CheckUserAccount(request.Email);
        await CheckRefreshToken(request.Token, user);
        try
        {
            var token = new Token();
            token.GenerateTokenValue(_settings, user.UserName!);
            return new UserClient
            {
                Id = user.Id,
                Name = _protector.Unprotect(user.UserName!),
                Email = _protector.UnprotectEmail(user.Email!),
                ImagePath = user.ImagePath,
                Session = new Session { Token = token, RefreshToken = request.Token },
            };
        }
        catch (ApplicationException exception)
        {
            _logger.LogError(
                LogEvents.PROPERTY_MISSING,
                exception,
                "Important settings to generate JWT are missing"
            );
            throw new AuthenticationException(
                ErrorCodes.CONFIG_NOT_FOUND,
                "Important settings to generate JWT are missing",
                exception
            );
        }
    }

    /// <summary>
    /// This method creates a new <c>RefreshToken</c>. Thereby the provided <c>user</c> instance will be updated
    /// as well as the new token inserted to the authentication database. This method will throw an
    /// <c><see cref="AuthenticationException"/></c> if one of the following occurs:
    /// <list type="bullet">
    ///     <item>The corresponding user account could not be updated (<see cref="ErrorCodes"/>: <c>UPDATE_FAILED</c>).</item>
    ///     <item>The generated refresh token could not be stored (<see cref="ErrorCodes"/>: <c>INSERT_FAILED</c>).</item>
    ///     <item>Necessary database operations have been canceled (<see cref="ErrorCodes"/>: <c>DATABASE_ERROR</c>).</item>
    /// </list>
    /// </summary>
    /// <param name="user">
    /// The user account from the authentication database.
    /// </param>
    /// <returns>
    /// The new created token.
    /// </returns>
    /// <exception cref="AuthenticationException">
    /// See method description.
    /// </exception>
    private async Task<RefreshToken> CreateRefreshToken(UserDatabase user)
    {
        // Generate new token
        var refreshToken = new RefreshToken
        {
            Id = new Guid().ToString(),
            UserId = user.Id,
            Token = "",
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            CreatedAt = DateTime.UtcNow,
        };
        refreshToken.GenerateTokenValue();
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
                    LogEvents.INSERT_FAILED,
                    "Refresh token could not be inserted for user - {user}",
                    user.Id
                );
                throw new AuthenticationException(
                    ErrorCodes.INSERT_FAILED,
                    "Failed to create refresh token"
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
                LogEvents.OPERATION_FAILED,
                exception,
                "Database operation has been canceled"
            );
            throw new AuthenticationException(
                ErrorCodes.DATABASE_ERROR,
                "Database operation has been canceled",
                exception
            );
        }
    }
}
