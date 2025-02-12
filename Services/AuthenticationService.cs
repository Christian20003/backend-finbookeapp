using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FinBookeAPI.Services;

public class AuthenticationService(
    UserManager<UserDatabase> userManager,
    SignInManager<UserDatabase> signInManager,
    IOptions<JwTSettings> settings,
    IDataProtectionProvider protector,
    ILogger<AuthenticationService> logger
) : IAuthenticationService
{
    private readonly UserManager<UserDatabase> _userManager = userManager;
    private readonly SignInManager<UserDatabase> _signInManager = signInManager;
    private readonly IOptions<JwTSettings> _settings = settings;
    private readonly IDataProtector _protector = protector.CreateProtector("user-data");
    private readonly ILogger<AuthenticationService> _logger = logger;

    public async Task<UserClient> Login(UserLogin data)
    {
        //Test.LogInformation("Hello World");
        // Proof if account exist
        var email = _protector.Protect(data.Email);
        var databaseUser =
            await _userManager.FindByEmailAsync(email)
            ?? throw new AuthenticationException("User not found", ErrorCodes.ENTRY_NOT_FOUND);
        // Proof if password is valid
        var _ =
            await _signInManager.CheckPasswordSignInAsync(
                databaseUser,
                data.Password,
                lockoutOnFailure: true
            )
            ?? throw new AuthenticationException("Password not correct", ErrorCodes.INVALID_ENTRY);
        // Proof if attributes of user exist
        if (databaseUser.UserName == null || databaseUser.Email == null)
        {
            throw new AuthenticationException(
                "Empty user name or email address",
                ErrorCodes.INVALID_ENTRY
            );
        }
        // Generate new token and user object
        var name = _protector.Unprotect(databaseUser.UserName);
        var token = new Token(name, _settings);
        return new UserClient
        {
            Id = databaseUser.Id,
            Name = _protector.Unprotect(databaseUser.UserName),
            Email = _protector.Unprotect(databaseUser.Email),
            ImagePath = databaseUser.ImagePath,
            Session = new Session(token.Value, token.Expires),
        };
    }

    public Task Logout()
    {
        throw new NotImplementedException();
    }

    public Task<UserClient> Register(UserRegister data, UserManager<UserDatabase> userManager)
    {
        throw new NotImplementedException();
    }

    public Task ResetPassword(string email, string code, UserManager<UserDatabase> userManager)
    {
        throw new NotImplementedException();
    }

    public Task SecurityCode(string email)
    {
        throw new NotImplementedException();
    }

    public Token GenerateToken()
    {
        throw new NotImplementedException();
    }
}
