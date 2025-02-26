using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Authentication.Interfaces;

namespace FinBookeAPI.Services.Authentication;

public interface IAuthenticationService
{
    /// <summary>
    /// This method implements the login for this application.
    /// </summary>
    /// <param name="data">
    /// Object which contains the login data received from the client.
    /// </param>
    /// <returns>
    /// A client object which contains all relevant information which can be sent to the client.
    /// </returns>
    /// <exception cref="AuthenticationException">
    /// If the login proccess fails.
    /// </exception>
    public Task<IUserClient> Login(IUserLogin data);

    /*
        1. Does email or username already exist
    */
    public Task<IUserClient> Register(IUserRegister data);

    /*
        1. Check if email exist
        2. Generate random code (6-digits)
        3. Send to client to provided email
    */
    public Task SecurityCode(IUserResetRequest request);

    /*
        1. Check if email exist
        2. Check if code exist
        3. Check if code is valid
        4. Generate random new password
        5. Send to client to provided email
    */
    public Task ResetPassword(IUserResetRequest request);

    /*
        1. Check if user exist
        2. Check if refresh token is valid
        3. Generate new random JWT token
        4. Send back to client
    */
    public Task<IUserClient> GenerateToken(IUserTokenRequest request);

    /// <summary>
    /// This method executes a logout request. The user will be logged out if
    /// the received refresh token is valid and corresponds to the provided
    /// user account.
    /// </summary>
    /// <param name="email">
    /// The email of the user which should be logged out.
    /// </param>
    /// <exception cref="AuthenticationException">
    /// If the logout proccess fails.
    /// </exception>
    public Task Logout(IUserTokenRequest request);
}
