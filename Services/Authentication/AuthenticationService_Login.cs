using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Authentication.Interfaces;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public async Task<IUserClient> Login(IUserLogin data)
    {
        _logger.LogDebug("Login call of {user}", data.Email);

        var email = _protector.Protect(data.Email);
        var user = await CheckUserAccount(email);
        await CheckPassword(user, data.Password);

        try
        {
            // Proof if refresh token exist and create a new one if not
            _logger.LogDebug("Check if refresh token exist of {user}", data.Email);
            var refreshToken = await _database.FindRefreshToken(token => token.UserId == user.Id);
            refreshToken ??= await CreateRefreshToken(user);

            // Generate new token and user object
            _logger.LogDebug("Create user object to be sent to the user");
            var name = _protector.Unprotect(user.UserName ?? "");
            var token = new Token();
            token.GenerateTokenValue(_settings, name);

            _logger.LogInformation(
                LogEvents.SUCCESSFUL_LOGIN,
                "A successful login from {Id}",
                user.Id
            );
            return new UserClient
            {
                Id = user.Id,
                Name = _protector.Unprotect(user.UserName ?? ""),
                Email = _protector.Unprotect(user.Email ?? ""),
                ImagePath = user.ImagePath,
                Session = new Session { Token = token, RefreshToken = refreshToken },
            };
        }
        catch (OperationCanceledException exception)
        {
            _logger.LogError(
                LogEvents.FAILED_OPERATION,
                exception,
                "Database operation has been canceled"
            );
            throw new AuthenticationException(
                "Database operation has been canceled",
                ErrorCodes.DATABASE_ERROR,
                exception
            );
        }
        catch (ApplicationException exception)
        {
            _logger.LogError(
                LogEvents.MISSING_PROPERTY,
                exception,
                "Important settings to generate JWT are missing"
            );
            throw new AuthenticationException(
                "Important settings to generate JWT are missing",
                ErrorCodes.CONFIG_NOT_FOUND,
                exception
            );
        }
    }

    /// <summary>
    /// This method proofs if the provided password corresponds to the user account and is valid. This method will
    /// throw an <c><see cref="AuthenticationException"/></c> if one of the following occurs:
    /// <list type="bullet">
    ///     <item>The provided password is not correct (<see cref="ErrorCodes"/>: <c>ACCESS_DENIED</c>).</item>
    ///     <item>The user is locked out for any authentication attempt (<see cref="ErrorCodes"/>: <c>ACCESS_DENIED</c>).</item>
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
                LogEvents.UNAUTHORIZED,
                "Provided password of user {id} is not valid",
                user.Id
            );
            throw new AuthenticationException("Password not correct", ErrorCodes.ACCESS_DENIED);
        }
        else if (check == SignInResult.LockedOut)
        {
            _logger.LogWarning(LogEvents.UNAUTHORIZED, "Login attempt with lockout restriction");
            throw new AuthenticationException(
                "Unauthorized login attempt",
                ErrorCodes.ACCESS_DENIED
            );
        }
    }
}
