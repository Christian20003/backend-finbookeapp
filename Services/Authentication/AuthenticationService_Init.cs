using FinBookeAPI.AppConfig;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService(
    UserManager<UserDatabase> userManager,
    SignInManager<UserDatabase> signInManager,
    AuthDbContext database,
    IOptions<IJwtSettings> settings,
    IDataProtection protection,
    ILogger<AuthenticationService> logger
) : IAuthenticationService
{
    private readonly UserManager<UserDatabase> _userManager = userManager;
    private readonly SignInManager<UserDatabase> _signInManager = signInManager;
    private readonly AuthDbContext _database = database;
    private readonly IOptions<IJwtSettings> _settings = settings;
    private readonly IDataProtection _protector = protection;
    private readonly ILogger<AuthenticationService> _logger = logger;
}
