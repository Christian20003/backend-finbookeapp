using FinBookeAPI.AppConfig;
using FinBookeAPI.AppConfig.Authentication;
using FinBookeAPI.AppConfig.Database;
using FinBookeAPI.AppConfig.Documentation;
using FinBookeAPI.AppConfig.Mapping;
using FinBookeAPI.AppConfig.Redaction;
using FinBookeAPI.Collections.CategoryCollection;
using FinBookeAPI.Collections.TokenCollection;
using FinBookeAPI.Middleware;
using FinBookeAPI.Models.Wrapper;
using FinBookeAPI.Services.Authentication;
using FinBookeAPI.Services.Email;
using FinBookeAPI.Services.SecurityUtility;
using FinBookeAPI.Services.Token;
using Microsoft.Extensions.Compliance.Redaction;

var builder = WebApplication.CreateBuilder(args);

// Add app configurations.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddConfig(builder.Configuration);
builder.Services.AddSwagger();
builder.Services.AddLogger(builder.Configuration);
builder.Services.AddRedactionExt();
builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddDbContext<DataDbContext>();
builder.Services.AddSecurity(builder.Configuration);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Wrapper
builder.Services.AddSingleton<IDataProtection, DataProtection>();
builder.Services.AddSingleton<IRedactorProvider, StarRedactorProvider>();
builder.Services.AddScoped<IAccountManager, AccountManager>();

// Collections
builder.Services.AddScoped<ITokenCollection, TokenCollection>();

// Services that provides additional functionality
builder.Services.AddScoped<ISecurityUtilityService, SecurityUtilityService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Services that provides key functionality
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ICategoryCollection, CategoryCollection>();
builder.Services.AddTransient<ExceptionHandling>();
builder.Services.AddTransient<BadRequestHandling>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger();
}
app.UseMiddleware<BadRequestHandling>();
app.UseMiddleware<ExceptionHandling>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
