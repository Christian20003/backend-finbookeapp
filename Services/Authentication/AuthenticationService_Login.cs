using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public async Task<UserClient> Login(UserLogin data)
    {
        _logger.LogDebug("Login call of {user}", data.Email);
        var user = await CheckUserAccount(data.Email);
        await CheckPassword(user, data.Password);
        try
        {
            // Proof if refresh token exist and create a new one if not
            _logger.LogDebug("Check if refresh token exist of {user}", data.Email);
            var refreshToken = await _database.FindRefreshToken(token => token.UserId == user.Id);
            refreshToken ??= await CreateRefreshToken(user);

            // Generate new token and user object
            _logger.LogDebug("Create user object to be sent to the user");
            var name = _protector.Unprotect(user.UserName!);
            var token = new Token();
            token.GenerateTokenValue(_settings, name);

            _logger.LogInformation(
                LogEvents.OPERATION_SUCCESS,
                "A successful login from {Id}",
                user.Id
            );
            return new UserClient
            {
                Id = user.Id,
                Name = _protector.Unprotect(user.UserName!),
                Email = _protector.UnprotectEmail(user.Email!),
                ImagePath = user.ImagePath,
                Session = new Session { Token = token, RefreshToken = refreshToken },
            };
        }
        catch (OperationCanceledException exception)
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
    /// This method proofs if the provided password corresponds to the user account and is valid. This method will
    /// throw an <c><see cref="AuthenticationException"/></c> if one of the following occurs:
    /// <list type="bullet">
    ///     <item>The provided password is not correct (<see cref="ErrorCodes"/>: <c>INVALID_CREDENTIALS</c>).</item>
    ///     <item>The user is locked out for any authentication attempt (<see cref="ErrorCodes"/>: <c>ACCESS_LOCKED</c>).</item>
    /// </list>
    /// </summary>
    /// <param name="user">
    /// The user account.
    /// </param>
    /// <param name="password">
    /// The password received from a client.
    /// </param>
    /// <exception cref="AuthenticationException">
    /// See method description.
    /// </exception>
    private async Task CheckPassword(UserDatabase user, string password)
    {
        _logger.LogDebug("Check provided user password of {user}", user.Email);
        var check = await _signInManager.CheckPasswordSignInAsync(
            user,
            password,
            lockoutOnFailure: true
        );
        if (check == SignInResult.Failed)
        {
            _logger.LogWarning(
                LogEvents.PROPERTY_INVALID,
                "Provided password of user {id} is not valid",
                user.Id
            );
            throw new AuthenticationException(
                ErrorCodes.INVALID_CREDENTIALS,
                "Password not correct"
            );
        }
        else if (check == SignInResult.LockedOut)
        {
            _logger.LogWarning(
                LogEvents.OPERATION_FAILED,
                "Login attempt with lockout restriction"
            );
            throw new AuthenticationException(
                ErrorCodes.ACCESS_LOCKED,
                "Unauthorized login attempt"
            );
        }
    }
}
