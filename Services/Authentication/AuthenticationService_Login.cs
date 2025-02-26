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
        // Proof if account exist
        _logger.LogDebug("Check existence of {user}", data.Email);
        var email = _protector.Protect(data.Email);
        var user = await CheckUserAccount(email);

        // Proof if password is valid
        _logger.LogDebug("Check correctness of password from {user}", data.Email);
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
                ErrorCodes.OPERATION_CANCELED,
                exception
            );
        }
    }

    /// <summary>
    /// This method proof if the provided password is valid to a corresponding user account.
    /// </summary>
    /// <param name="user">
    /// The account of the user.
    /// </param>
    /// <param name="password">
    /// The provided password.
    /// </param>
    /// <exception cref="AuthenticationException">
    /// If the password is not valid or the user is not authorized to login.
    /// </exception>
    private async Task CheckPassword(UserDatabase user, string password)
    {
        var check = await _signInManager.CheckPasswordSignInAsync(
            user,
            password,
            lockoutOnFailure: true
        );
        if (check == SignInResult.Failed)
        {
            _logger.LogWarning(
                LogEvents.FAILED_CHECK,
                "Provided password of user {id} is not valid",
                user.Id
            );
            throw new AuthenticationException("Password not correct", ErrorCodes.INVALID_ENTRY);
        }
        else if (check == SignInResult.LockedOut)
        {
            _logger.LogWarning(LogEvents.UNAUTHORIZED, "Login attempt with lockout restriction");
            throw new AuthenticationException(
                "Unauthorized login attempt",
                ErrorCodes.UNAUTHORIZED
            );
        }
    }
}
