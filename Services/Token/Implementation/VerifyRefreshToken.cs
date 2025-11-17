using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FinBookeAPI.Services.Token;

public partial class TokenService : ITokenService
{
    public string VerifyRefreshToken(string token)
    {
        _logger.LogDebug("Verify refresh token {token}", token);
        var audience =
            _settings.Value.Audience
            ?? throw new ApplicationException("Audience configuration is null");
        var issuer =
            _settings.Value.Issuer
            ?? throw new ApplicationException("Issuer configuration is null");
        var secret =
            _settings.Value.RefreshTokenSecret
            ?? throw new ApplicationException("Refresh token secret is null");
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParam = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ClockSkew = TimeSpan.Zero,
        };
        var principles = tokenHandler.ValidateToken(token, validationParam, out _);
        var claim = principles.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
        return claim.Value;
    }
}
