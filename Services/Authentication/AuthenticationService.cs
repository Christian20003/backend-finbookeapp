using FinBookeAPI.AppConfig;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Wrapper;
using FinBookeAPI.Services.Email;
using FinBookeAPI.Services.SecurityUtility;
using FinBookeAPI.Services.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Compliance.Redaction;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService(
    IAccountManager accountManager,
    SignInManager<UserAccount> signInManager,
    ISecurityUtilityService securityUtilityService,
    ITokenService tokenService,
    IEmailService emailService,
    IDataProtection protection,
    IRedactorProvider redactor,
    ILogger<AuthenticationService> logger
) : IAuthenticationService
{
    private readonly IAccountManager _accountManager = accountManager;
    private readonly SignInManager<UserAccount> _signInManager = signInManager;
    private readonly ISecurityUtilityService _securityUtilityService = securityUtilityService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IEmailService _emailService = emailService;
    private readonly IDataProtection _protector = protection;
    private readonly IRedactorProvider _redactor = redactor;
    private readonly ILogger<AuthenticationService> _logger = logger;
}
