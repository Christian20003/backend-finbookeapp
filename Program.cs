using FinBookeAPI.AppConfig;
using FinBookeAPI.Middleware;
using FinBookeAPI.Models.Wrapper;
using FinBookeAPI.Services.Authentication;

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

builder.Services.AddSingleton<IDataProtection, DataProtection>();
builder.Services.AddScoped<IAccountManager, AccountManager>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
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
