using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public async Task Logout(UserTokenRequest request)
    {
        _logger.LogDebug("Logout call of {user}", request.Email);
        var user = await CheckUserAccount(_protector.Protect(request.Email));
        await CheckRefreshToken(request.Token, user);
        try
        {
            _logger.LogDebug("Remove refresh token of {user}", request.Email);
            await _database.RemoveRefreshToken(user.RefreshTokenId);
            user.RefreshTokenId = "";
            await UpdateUser(user);
            await _database.SaveChangesAsync();
        }
        catch (NullReferenceException)
        {
            _logger.LogInformation(LogEvents.MISSING_OBJECT, "Refresh token not found");
            throw new AuthenticationException(
                "Refresh token not found",
                ErrorCodes.ENTRY_NOT_FOUND
            );
        }
        catch (Exception exception)
            when (exception is OperationCanceledException
                || exception is DbUpdateException
                || exception is DbUpdateConcurrencyException
            )
        {
            _logger.LogWarning(
                LogEvents.FAILED_OPERATION,
                "Deleting a refresh token has been canceled"
            );
            throw new AuthenticationException(
                "Database operation has been canceled",
                ErrorCodes.DATABASE_ERROR
            );
        }
    }
}
