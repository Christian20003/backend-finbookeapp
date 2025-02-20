using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public async Task<UserClient> Register(UserRegister data)
    {

        _logger.LogDebug("User Registrated, check email, username and co" + data);

        CheckUserInDb(data);


        var newUser = new UserDatabase
        {
            UserName = data.Name,
            Email = data.Email,
        };
        await _userManager.CreateAsync(newUser);
        await _userManager.AddPasswordAsync(newUser, data.Password);


        var databaseUser = await _userManager.FindByEmailAsync(newUser.Email);

        // Proof if refresh token exist and create a new one if not
        var refreshToken = databaseUser != null ? await _database.FindRefreshToken(doc => doc.UserId == databaseUser.Id) : null;
        if (refreshToken == null)
        {
            refreshToken = new RefreshToken
            {
                Id = new Guid().ToString(),
                UserId = databaseUser!.Id,
                Token = RefreshToken.GenerateToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow,
            };
            _logger.LogDebug("Generate new refresh token: {token}", refreshToken.Token);
            databaseUser.RefreshTokenId = refreshToken.Id;
            await _userManager.UpdateAsync(databaseUser);
            // Hash token for security
            using System.Security.Cryptography.SHA256 algo = System.Security.Cryptography.SHA256.Create();
            await _database.AddRefreshToken(
                new RefreshToken
                {
                    Id = refreshToken.Id,
                    UserId = refreshToken.UserId,
                    Token = GetHash(algo, refreshToken.Token),
                    ExpiresAt = refreshToken.ExpiresAt,
                    CreatedAt = refreshToken.CreatedAt,
                }
            );
            await _database.SaveChangesAsync();
        }


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

    private async void CheckUserInDb(UserRegister newUser)
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
