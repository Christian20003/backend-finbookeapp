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
        var user = await CheckUserAccount(request.Email);
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
            _logger.LogInformation(LogEvents.DELETE_FAILED, "Refresh token not found");
            throw new AuthenticationException(ErrorCodes.DELETE_FAILED, "Refresh token not found");
        }
        catch (Exception exception)
            when (exception is OperationCanceledException
                || exception is DbUpdateException
                || exception is DbUpdateConcurrencyException
            )
        {
            _logger.LogWarning(
                LogEvents.OPERATION_FAILED,
                "Deleting a refresh token has been canceled"
            );
            throw new AuthenticationException(
                ErrorCodes.DATABASE_ERROR,
                "Database operation has been canceled"
            );
        }
    }
}
