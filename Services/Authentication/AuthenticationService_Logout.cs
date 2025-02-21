using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public async Task Logout(string email)
    {
        _logger.LogDebug("Proof existence of user account: {user}", email);
        var user = await CheckUserAccount(_protector.Protect(email));
        try
        {
            _logger.LogDebug("Remove refresh token if it exist: {user}", email);
            await _database.RemoveRefreshToken(user.RefreshTokenId);
            _logger.LogDebug("Update user account in database: {user}", email);
            user.RefreshTokenId = "";
            await UpdateUser(user);
            await _database.SaveChangesAsync();
        }
        catch (NullReferenceException)
        {
            _logger.LogInformation(LogEvents.MISSING_OBJECT, "Refresh token is already deleted");
            return;
        }
        catch (Exception exception)
            when (exception is OperationCanceledException
                || exception is DbUpdateException
                || exception is DbUpdateConcurrencyException
            )
        {
            _logger.LogError(
                LogEvents.FAILED_OPERATION,
                "Deleting a refresh token has been canceled"
            );
            throw new AuthenticationException(
                "Database operation has been canceled",
                ErrorCodes.OPERATION_CANCELED
            );
        }
    }
}
