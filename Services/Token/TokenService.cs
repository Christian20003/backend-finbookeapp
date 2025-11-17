using FinBookeAPI.Models.Configuration;
using Microsoft.Extensions.Options;

namespace FinBookeAPI.Services.Token;

public partial class TokenService(IOptions<JwtSettings> settings, ILogger<TokenService> logger)
    : ITokenService
{
    private readonly IOptions<JwtSettings> _settings = settings;
    private readonly ILogger<TokenService> _logger = logger;
}
