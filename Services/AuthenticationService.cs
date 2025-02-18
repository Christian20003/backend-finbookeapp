using System.Security.Cryptography;
using System.Text;
using FinBookeAPI.AppConfig;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver.Linq;

namespace FinBookeAPI.Services;

public class AuthenticationService(
    UserManager<UserDatabase> userManager,
    SignInManager<UserDatabase> signInManager,
    AuthDbContext database,
    IOptions<JwTSettings> settings,
    IDataProtectionProvider protector,
    ILogger<AuthenticationService> logger
) : IAuthenticationService
{
    private readonly UserManager<UserDatabase> _userManager = userManager;
    private readonly SignInManager<UserDatabase> _signInManager = signInManager;
    private readonly AuthDbContext _database = database;
    private readonly IOptions<JwTSettings> _settings = settings;
    private readonly IDataProtector _protector = protector.CreateProtector("user-data");
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
        var _ =
            await _signInManager.CheckPasswordSignInAsync(
                databaseUser,
                data.Password,
                lockoutOnFailure: true
            )
            ?? throw new AuthenticationException("Password not correct", ErrorCodes.INVALID_ENTRY);

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
        var refreshToken = await _database.RefreshToken.FirstOrDefaultAsync(doc =>
            doc.UserId == databaseUser.Id
        );
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
            await _database.RefreshToken.AddAsync(
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
}
