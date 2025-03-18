using FinBookeAPI.Models.Authentication;

namespace FinBookeAPI.Services.Authentication;

public interface IAuthenticationService
{
    /// <summary>
    /// This method process a login attempt by using the provided login data. This method will throw an
    /// <c><see cref="AuthenticationException"/></c> if one of the following occurs:
    /// <list type="bullet">
    ///     <item>The provided email does not have a user account (<see cref="ErrorCodes"/>: <c>INVALID_CREDENTIALS</c>).</item>
    ///     <item>The provided password is not correct (<see cref="ErrorCodes"/>: <c>INVALID_CREDENTIALS</c>).</item>
    ///     <item>The found user account has an empty string as username property (<see cref="ErrorCodes"/>: <c>UNEXPECTED_STRUCTURE</c>).</item>
    ///     <item>The corresponding user account could not be updated (<see cref="ErrorCodes"/>: <c>UPDATE_FAILED</c>).</item>
    ///     <item>The generated refresh token could not be stored (<see cref="ErrorCodes"/>: <c>INSERT_FAILED</c>).</item>
    ///     <item>The user is locked out for any authentication attempt (<see cref="ErrorCodes"/>: <c>ACCESS_LOCKED</c>).</item>
    ///     <item>Important settings for generating tokens are missing (<see cref="ErrorCodes"/>: <c>CONFIG_NOT_FOUND</c>).</item>
    ///     <item>Necessary database operations have been canceled (<see cref="ErrorCodes"/>: <c>DATABASE_ERROR</c>).</item>
    /// </list>
    /// </summary>
    /// <param name="data">
    /// Object which contains the login data received from the client.
    /// </param>
    /// <returns>
    /// A client object which contains all relevant information which should be sent to the client
    /// for further authentication requests.
    /// </returns>
    /// <exception cref="AuthenticationException">
    /// See method description.
    /// </exception>
    public Task<UserClient> Login(UserLogin data);

    /*
        1. Does email or username already exist
    */
    public Task<UserClient> Register(UserRegister data);

    /// <summary>
    /// This method sends a new generated security code to the provided email through an SMTP-Server and stores the result as well
    /// in the authentication database. This method will throw an <c><see cref="AuthenticationException"/></c> if one of the
    /// following occurs:
    /// <list type="bullet">
    ///     <item>The provided email does not have a user account (<see cref="ErrorCodes"/>: <c>INVALID_CREDENTIALS</c>).</item>
    ///     <item>The found user account has an empty string as username property (<see cref="ErrorCodes"/>: <c>UNEXPECTED_STRUCTURE</c>).</item>
    ///     <item>The provided message could not be sent due to an SMTP-Server error (<see cref="ErrorCodes"/>: <c>EXTERNAL_SERVICE_ERROR</c>).</item>
    ///     <item>The corresponding user account could not be updated (<see cref="ErrorCodes"/>: <c>UPDATE_FAILED</c>).</item>
    /// </list>
    /// </summary>
    /// <param name="request">
    /// The object with an email address.
    /// </param>
    /// <exception cref="AuthenticationException">
    /// See method description.
    /// </exception>
    public Task SecurityCode(UserResetRequest request);

    /// <summary>
    /// This method resets the password of the user and sends the new random generated password via email to the user. This method
    /// will throw an <c><see cref="AuthenticationException"/></c> if one of the following occurs:
    /// <list type="bullet">
    ///     <item>The provided email does not have a user account (<see cref="ErrorCodes"/>: <c>INVALID_CREDENTIALS</c>).</item>
    ///     <item>The found user account has an empty string as username property (<see cref="ErrorCodes"/>: <c>UNEXPECTED_STRUCTURE</c>).</item>
    ///     <item>The user account does not have a valid security code property (<see cref="ErrorCodes"/>: <c>UNEXPECTED_STRUCTURE</c>).</item>
    ///     <item>The provided security code has expired (<see cref="ErrorCodes"/>: <c>EXPIRED_CODE</c>).</item>
    ///     <item>The provided security code is not correct (<see cref="ErrorCodes"/>: <c>INVALID_CODE</c>).</item>
    ///     <item>The provided message could not be sent due to an SMTP-Server error (<see cref="ErrorCodes"/>: <c>EXTERNAL_SERVICE_ERROR</c>).</item>
    ///     <item>The corresponding user account could not be updated (<see cref="ErrorCodes"/>: <c>UPDATE_FAILED</c>).</item>
    /// </list>
    /// </summary>
    /// <param name="request">
    /// The object with an email address and received security code.
    /// </param>
    /// <exception cref="AuthenticationException">
    /// See method description.
    /// </exception>
    public Task ResetPassword(UserResetRequest request);

    /// <summary>
    /// This method allows the user to get a new JWT. This method will throw an <c><see cref="AuthenticationException"/></c>
    /// if one of the following occurs:
    /// <list type="bullet">
    ///     <item>The provided email does not have a user account (<see cref="ErrorCodes"/>: <c>INVALID_CREDENTIALS</c>).</item>
    ///     <item>The found user account has an empty string as username property (<see cref="ErrorCodes"/>: <c>UNEXPECTED_STRUCTURE</c>).</item>
    ///     <item>The user account does not have a refresh token (<see cref="ErrorCodes"/>: <c>INVALID_TOKEN</c>).</item>
    ///     <item>The provided token does not correspond to the stored token (<see cref="ErrorCodes"/>: <c>INVALID_TOKEN</c>).</item>
    ///     <item>The stored token has expired (<see cref="ErrorCodes"/>: <c>EXPIRED_TOKEN</c>).</item>
    ///     <item>Important settings for generating tokens are missing (<see cref="ErrorCodes"/>: <c>CONFIG_NOT_FOUND</c>).</item>
    ///     <item>Necessary database operations have been canceled (<see cref="ErrorCodes"/>: <c>DATABASE_ERROR</c>).</item>
    /// </list>
    /// </summary>
    /// <param name="request">
    /// The object with an email address and refresh token.
    /// </param>
    /// <returns>
    /// A client object which contains all relevant information which should be sent to the client for further
    /// authentication requests.
    /// </returns>
    /// <exception cref="AuthenticationException">
    /// See method description.
    /// </exception>
    public Task<UserClient> GenerateToken(UserTokenRequest request);

    /// <summary>
    /// This method process a logout attempt. his method will throw an <c><see cref="AuthenticationException"/></c>
    /// if one of the following occurs:
    /// <list type="bullet">
    ///     <item>The provided email does not have a user account (<see cref="ErrorCodes"/>: <c>INVALID_CREDENTIALS</c>).</item>
    ///     <item>The found user account has an empty string as username property (<see cref="ErrorCodes"/>: <c>UNEXPECTED_STRUCTURE</c>).</item>
    ///     <item>The user account does not have a refresh token (<see cref="ErrorCodes"/>: <c>INVALID_TOKEN</c> or <c>DELETE_FAILED</c>).</item>
    ///     <item>The provided token does not correspond to the stored token (<see cref="ErrorCodes"/>: <c>INVALID_TOKEN</c>).</item>
    ///     <item>The stored token has expired (<see cref="ErrorCodes"/>: <c>EXPIRED_TOKEN</c>).</item>
    ///     <item>Necessary database operations have been canceled (<see cref="ErrorCodes"/>: <c>DATABASE_ERROR</c>).</item>
    /// </list>
    /// </summary>
    /// <param name="email">
    /// The email of the user which should be logged out.
    /// </param>
    /// <exception cref="AuthenticationException">
    /// See method description.
    /// </exception>
    public Task Logout(UserTokenRequest request);
}
