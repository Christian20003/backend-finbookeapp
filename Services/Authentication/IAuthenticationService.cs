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

    /// <summary>
    /// This method sends a new generated security code to the provided email through an
    /// SMTP-Server and stores the result as well in the authentication database.
    /// </summary>
    /// <param name="request">
    /// An object that contains all necessary information to sent an email.
    /// </param>
    /// <exception cref="AuthenticationException">
    /// If the security code generation proccess fails.
    /// </exception>
    public Task SecurityCode(IUserResetRequest request);

    /// <summary>
    /// This method resets the password of the user and sends the new random generated password
    /// via email to the user.
    /// </summary>
    /// <param name="request">
    /// An object that contains all necessary information to to reset the password.
    /// </param>
    /// <exception cref="AuthenticationException">
    /// If the reset password proccess fails.
    /// </exception>
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
