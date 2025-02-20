using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public async Task<UserClient> Login(UserLogin data)
    {
        _logger.LogDebug("Check existence of {user}", data.Email);
        // Proof if account exist
        var email = _protector.Protect(data.Email);
        var databaseUser =
            await _userManager.FindByEmailAsync(email)
            ?? throw new AuthenticationException("User not found", ErrorCodes.ENTRY_NOT_FOUND);
        // Proof if password is valid

        _logger.LogDebug("Check correctness of password from {user}", data.Email);

        var check = await _signInManager.CheckPasswordSignInAsync(
            databaseUser,
            data.Password,
            lockoutOnFailure: true
        );
        if (check == SignInResult.Failed)
        {
            throw new AuthenticationException("Password not correct", ErrorCodes.INVALID_ENTRY);
        }

        _logger.LogDebug("Check existence of name and email from {Id}", databaseUser.Id);
        // Proof if attributes of user exist
        if (databaseUser.UserName == null || databaseUser.Email == null)
        {
            _logger.LogWarning(
                LogEvents.MISSING_PROPERTY,
                "Unexpected missing of username or email from {Id}",
                databaseUser.Id
            );
            throw new AuthenticationException(
                "Empty user name or email address",
                ErrorCodes.INVALID_ENTRY
            );
        }

        // Proof if refresh token exist and create a new one if not
        var refreshToken = await _database.FindRefreshToken(doc => doc.UserId == databaseUser.Id);
        refreshToken ??= await CreateRefreshToken(databaseUser);

        _logger.LogDebug("Create user object to be sent to the user");
        // Generate new token and user object
        var name = _protector.Unprotect(databaseUser.UserName);
        var token = new Token(name, _settings);

        _logger.LogInformation(
            LogEvents.SUCCESSFUL_LOGIN,
            "A successful login from {Id}",
            databaseUser.Id
        );
        return new UserClient
        {
            Id = databaseUser.Id,
            Name = _protector.Unprotect(databaseUser.UserName),
            Email = _protector.Unprotect(databaseUser.Email),
            ImagePath = databaseUser.ImagePath,
            Session = new Session { Token = token, RefreshToken = refreshToken },
        };
    }
}
