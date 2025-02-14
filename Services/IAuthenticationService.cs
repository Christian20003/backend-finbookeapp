using FinBookeAPI.Models.Authentication;
using Microsoft.AspNetCore.Identity;

namespace FinBookeAPI.Services;

public interface IAuthenticationService
{
    /// <summary>
    /// This method implements the login for this application.
    /// </summary>
    /// <param name="data">Object which contains the login data received from the client.</param>
    /// <returns>A client object which contains all relevant information which can be sent to the client.</returns>
    /// <exception cref="AuthenticationException">If the login proccess fails</exception>
    public Task<UserClient> Login(UserLogin data);

    public Task<UserClient> Register(UserRegister data, UserManager<UserDatabase> userManager);

    public Task Logout();

    public Task SecurityCode(string email);

    public Task ResetPassword(string email, string code, UserManager<UserDatabase> userManager);

    public Token GenerateToken();
}
