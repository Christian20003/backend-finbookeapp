using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public async Task<UserClient> Login(IUserLogin data)
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
            var token = new Token(name, _settings);

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
    private async Task<IUserDatabase> CheckUserAccount(string email)
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
    private async Task CheckPassword(IUserDatabase user, string password)
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
