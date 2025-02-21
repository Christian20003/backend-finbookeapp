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
    public Task<UserClient> Login(IUserLogin data);

    /*
        1. Does email or username already exist
    */
    public Task<UserClient> Register(UserRegister data);

    /*
        1. Check if email exist
        2. Generate random code (6-digits)
        3. Send to client to provided email
    */
    public void SecurityCode(string email);

    /*
        1. Check if email exist
        2. Check if code exist
        3. Check if code is valid
        4. Generate random new password
        5. Send to client to provided email
    */
    public void ResetPassword(string email, string code);

    /*
        1. Check if user exist
        2. Check if refresh token is valid
        3. Generate new random JWT token
        4. Send back to client
    */
    public UserClient GenerateToken(UserClient data);

    /*
        1. Check if user exist
        2. Delete refresh token
    */
    public void Logout(UserClient data);
}
