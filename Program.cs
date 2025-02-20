using FinBookeAPI.AppConfig;
using FinBookeAPI.Models.Wrapper;
using FinBookeAPI.Services;
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

builder.Services.AddScoped<IDataProtection, DataProtection>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
