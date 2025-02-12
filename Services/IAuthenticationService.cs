using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FinBookeAPI.Services;

public interface IAuthenticationService
{
    /// <summary>
    /// This method implements the login procedure for this application with JWT.
    /// </summary>
    /// <param name="data">Object which contains the login data received from the client.</param>
    public Task<UserClient> Login(UserLogin data);

    public Task<UserClient> Register(UserRegister data, UserManager<UserDatabase> userManager);

    public Task Logout();

    public Task SecurityCode(string email);

    public Task ResetPassword(string email, string code, UserManager<UserDatabase> userManager);

    public Token GenerateToken();
}
