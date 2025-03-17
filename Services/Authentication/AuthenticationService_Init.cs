using FinBookeAPI.AppConfig;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService(
    IAccountManager accountManager,
    SignInManager<UserDatabase> signInManager,
    AuthDbContext database,
    IOptions<JwtSettings> settings,
    IOptions<SmtpServer> mailServer,
    IDataProtection protection,
    ILogger<AuthenticationService> logger
) : IAuthenticationService
{
    private readonly IAccountManager _accountManager = accountManager;
    private readonly SignInManager<UserDatabase> _signInManager = signInManager;
    private readonly AuthDbContext _database = database;
    private readonly IOptions<JwtSettings> _settings = settings;
    private readonly IOptions<SmtpServer> _mailServer = mailServer;
    private readonly IDataProtection _protector = protection;
    private readonly ILogger<AuthenticationService> _logger = logger;
}
