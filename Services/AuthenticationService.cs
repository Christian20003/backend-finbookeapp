using System.Security.Cryptography;
using System.Text;
using FinBookeAPI.AppConfig;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using FinBookeAPI.Models.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FinBookeAPI.Services;

public class AuthenticationService(
    UserManager<UserDatabase> userManager,
    SignInManager<UserDatabase> signInManager,
    AuthDbContext database,
    IOptions<JwTSettings> settings,
    IDataProtection protection,
    ILogger<AuthenticationService> logger
) : IAuthenticationService
{
    private readonly UserManager<UserDatabase> _userManager = userManager;
    private readonly SignInManager<UserDatabase> _signInManager = signInManager;
    private readonly AuthDbContext _database = database;
    private readonly IOptions<JwTSettings> _settings = settings;
    private readonly IDataProtection _protector = protection;
    private readonly ILogger<AuthenticationService> _logger = logger;

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
        if (refreshToken == null)
        {
            refreshToken = new RefreshToken
            {
                Id = new Guid().ToString(),
                UserId = databaseUser.Id,
                Token = RefreshToken.GenerateToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow,
            };
            _logger.LogDebug("Generate new refresh token: {token}", refreshToken.Token);
            databaseUser.RefreshTokenId = refreshToken.Id;
            await _userManager.UpdateAsync(databaseUser);
            // Hash token for security
            using SHA256 algo = SHA256.Create();
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
        return new UserClient
        {
            Id = databaseUser.Id,
            Name = _protector.Unprotect(databaseUser.UserName),
            Email = _protector.Unprotect(databaseUser.Email),
            ImagePath = databaseUser.ImagePath,
            Session = new Session { Token = token, RefreshToken = refreshToken },
        };
    }

    private static string GetHash(HashAlgorithm algorithm, string input)
    {
        var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
        var builder = new StringBuilder();
        foreach (var elem in hash)
        {
            builder.Append(elem.ToString("x2"));
        }
        return builder.ToString();
    }

    public Task<UserClient> Register(UserRegister data)
    {
        throw new NotImplementedException();
    }

    public void SecurityCode(string email)
    {
        throw new NotImplementedException();
    }

    public void ResetPassword(string email, string code)
    {
        throw new NotImplementedException();
    }

    public UserClient GenerateToken(UserClient data)
    {
        throw new NotImplementedException();
    }

    public void Logout(UserClient data)
    {
        throw new NotImplementedException();
    }
}
