using FinBookeAPI.AppConfig;
using FinBookeAPI.Middleware;
using FinBookeAPI.Models.Wrapper;
//using FinBookeAPI.Services.Authentication;
using FinBookeAPI.Services.Email;
using FinBookeAPI.Services.GenHash;

var builder = WebApplication.CreateBuilder(args);

// Add app configurations.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddConfig(builder.Configuration);
builder.Services.AddSwagger();
builder.Services.AddLogger(builder.Configuration);
builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddDbContext<DataDbContext>();
builder.Services.AddSecurity(builder.Configuration);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Wrapper
builder.Services.AddSingleton<IDataProtection, DataProtection>();
builder.Services.AddScoped<IAccountManager, AccountManager>();

// Services
// builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IGenHashService, GenHashService>();
builder.Services.AddScoped<IEmailService, EmailService>();
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
