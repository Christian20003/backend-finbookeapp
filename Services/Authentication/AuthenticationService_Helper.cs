using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Authentication.Interfaces;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    /// <summary>
    /// This method proofs if any user account exist in the authentication database from the provided email.
    /// </summary>
    /// <param name="email">
    /// The email of the user account.
    /// </param>
    /// <returns>
    /// An instance of <c>UserDatabase</c> which represents a single user account.
    /// </returns>
    /// <exception cref="AuthenticationException">
    /// If not any user account could be found or the found instance has an empty username or email property.
    /// </exception>
    private async Task<UserDatabase> CheckUserAccount(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        // Proof if user exist
        if (user == null)
        {
            _logger.LogWarning(LogEvents.FAILED_SEARCH, "User account could not be found");
            throw new AuthenticationException("User not found", ErrorCodes.ENTRY_NOT_FOUND);
        }
        // Proof if username property is set
        if (user.UserName == null)
        {
            _logger.LogWarning(
                LogEvents.MISSING_PROPERTY,
                "Unexpected missing username property of user {id}",
                user.Id
            );
            throw new AuthenticationException("Empty username", ErrorCodes.INVALID_ENTRY);
        }
        // Proof if email property is set
        if (user.Email == null)
        {
            _logger.LogWarning(
                LogEvents.MISSING_PROPERTY,
                "Unexpected missing email property of user {id}",
                user.Id
            );
            throw new AuthenticationException("Empty email", ErrorCodes.INVALID_ENTRY);
        }
        return user;
    }

    /// <summary>
    /// This method proofs if the user has provided a valid refresh token which has been
    /// assigned to his account.
    /// </summary>
    /// <param name="token">
    /// The refresh token received from the user.
    /// </param>
    /// <param name="user">
    /// The user account from the database
    /// </param>
    /// <exception cref="AuthenticationException">
    /// If the user does not have any refresh tokens or the refresh token is invalid. It can
    /// also happen if the database cancel any of its operations.
    /// </exception>
    private async Task CheckRefreshToken(IRefreshToken token, UserDatabase user)
    {
        try
        {
            var storedToken = await _database.FindRefreshToken(obj =>
                obj.Id == user.RefreshTokenId
            );
            if (storedToken == null)
            {
                _logger.LogInformation(LogEvents.MISSING_OBJECT, "Refresh token not found");
                throw new AuthenticationException(
                    "Refresh token not found",
                    ErrorCodes.ENTRY_NOT_FOUND
                );
            }
            token.HashValue();
            if (storedToken.Token != token.Token)
            {
                _logger.LogWarning(
                    LogEvents.UNAUTHORIZED,
                    "Invalid refresh token provided for logout"
                );
                throw new AuthenticationException("Invalid refresh token", ErrorCodes.UNAUTHORIZED);
            }
        }
        catch (OperationCanceledException exception)
        {
            _logger.LogError(LogEvents.FAILED_OPERATION, "Database operation has been canceled");
            throw new AuthenticationException(
                "Database operation has been canceled",
                ErrorCodes.OPERATION_CANCELED,
                exception
            );
        }
    }

    /// <summary>
    /// This method updates a user account in the database.
    /// </summary>
    /// <param name="user">
    /// The user object with all updated properties.
    /// </param>
    /// <exception cref="AuthenticationException">
    /// If the update fails.
    /// </exception>
    private async Task UpdateUser(UserDatabase user)
    {
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
    }
}
