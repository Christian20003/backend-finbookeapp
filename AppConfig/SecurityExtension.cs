using System.Text;
using FinBookeAPI.Models.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace FinBookeAPI.AppConfig;

public static class SecurityExtension
{
    /// <summary>
    /// This method adds a authentication provider to this application, specifically <c>JWT</c>.
    /// </summary>
    /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
    /// <param name="_configuration">A set of key/value application configuration properties.</param>
    /// <returns>The modified <c>IServiceCollection</c></returns>
    /// <exception cref="ApplicationException">If necessary settings from the <c>appsettings.json</c> file are missing.</exception>
    public static IServiceCollection AddSecurity(
        this IServiceCollection services,
        IConfiguration _configuration
    )
    {
        // Add an identity system
        services
            .AddIdentityCore<DbUser>()
            .AddSignInManager()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        // Add authentication restrictions
        services.Configure<IdentityOptions>(options =>
        {
            // Password restrictions
            options.Password.RequiredLength = 10;
            options.Password.RequiredUniqueChars = 5;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;

            // User restrictions
            options.User.RequireUniqueEmail = true;

            // Restricted number of tries
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        });

        // Add authentication provider
        var authBuilder = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        });
        authBuilder.AddJwtBearer(options =>
        {
            var jwt = _configuration.GetSection("JwtConfig");
            var issuer = jwt["ValidIssuer"];
            var audience = jwt["ValidAudience"];
            var secret = jwt["Secret"];
            if (issuer == null || audience == null || secret == null)
            {
                throw new ApplicationException("Missing authentication data in configuration");
            }
            options.SaveToken = true;
            //TODO: Should be changed after development
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidIssuer = issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            };
        });
        return services;
    }
}
