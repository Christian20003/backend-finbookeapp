using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Authentication.Interfaces;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public async Task<IUserClient> Register(IUserRegister data)
    {
        _logger.LogDebug("User Registrated, check email, username and co" + data);

        CheckUserInDb(data);

        var newUser = new UserDatabase { UserName = data.Name, Email = data.Email };
        await _userManager.CreateAsync(newUser);
        await _userManager.AddPasswordAsync(newUser, data.Password);

        var databaseUser = await _userManager.FindByEmailAsync(newUser.Email);

        // Proof if refresh token exist and create a new one if not
        var refreshToken =
            databaseUser != null
                ? await _database.FindRefreshToken(doc => doc.UserId == databaseUser.Id)
                : null;
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

        // error handling


        return new UserClient
        {
            Id = databaseUser.Id,
            Name = _protector.Unprotect(databaseUser.UserName),
            Email = _protector.Unprotect(databaseUser.Email),
            ImagePath = databaseUser.ImagePath,
            Session = new Session { Token = token, RefreshToken = refreshToken },
        };
    }

    private async void CheckUserInDb(IUserRegister newUser)
    {
        var test1 = await _userManager.FindByNameAsync(newUser.Email);
        var test2 = await _userManager.FindByNameAsync(newUser.Name);
        var isValid = test1 == null && test2 == null;

        if (!isValid)
        {
            throw new AuthenticationException("User alread exist", ErrorCodes.INVALID_ENTRY);
        }
    }
}
