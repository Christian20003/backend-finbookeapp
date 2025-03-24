using FinBookeAPI.DTO.Authentication;
using FinBookeAPI.DTO.Error;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace FinBookeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(
    ILogger<AuthenticationController> logger,
    IAuthenticationService service
) : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger = logger;
    private readonly IAuthenticationService _service = service;

    /// <summary>
    /// This method process a login request and proofs if the user has access to his profile.
    /// </summary>
    /// <param name="data">
    /// The login data to authenticate the user.
    /// </param>
    /// <returns>A session object including all user data stored on the client side</returns>
    /// <response code="200">If the user login was successful</response>
    /// <response code="400">If the received data does not fulfill the requirements</response>
    /// <response code="403">If the user provided invalid credentials</response>
    /// <response code="406">If the user account does not have a valid username</response>
    /// <response code="423">If the user is locked due to incorrect login attemps</response>
    /// <response code="500">If any other kind of server error occur</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(UserDTO), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    [ProducesResponseType(typeof(ErrorResponse), 406)]
    [ProducesResponseType(typeof(ErrorResponse), 423)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO data)
    {
        _logger.LogInformation(LogEvents.INCOMING_REQUEST, "New login request");
        var result = await _service.Login(data.GetUserLogin());
        var response = new UserDTO(result);
        return Ok(response);
    }

    /// <summary>
    /// This method process a register request by generating a new user account.
    /// </summary>
    /// <param name="userRegData">
    /// The data to create a new user account.
    /// </param>
    /// <returns>A session object including all user data stored on the client side</returns>
    /// <response code="201">If the registration works successfully</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(UserDTO), 201)]
    public async Task<ActionResult<UserDTO>> UserRegistration([FromBody] RegisterDTO userRegData)
    {
        var user = await _service.Register(userRegData.GetUserRegister());
        var response = new UserDTO(user);
        return CreatedAtAction("Registration successful", response);
    }

    /// <summary>
    /// This method process a logout request by cancelling the current session.
    /// </summary>
    /// <param name="data">
    /// The data to verify the user for its logout attempt.
    /// </param>
    /// <response code="200">If the logout request was successful</response>
    /// <response code="400">If the received data does not fulfill the requirements</response>
    /// <response code="403">
    /// If the user provided an invalid refresh token or if it has expired.
    /// If the user provided an invalid email address (not assignable to any account)
    /// </response>
    /// <response code="406">If the user account does not have a valid username</response>
    /// <response code="500">If any other kind of server error occur</response>
    [HttpPost("logout")]
    [ProducesResponseType(typeof(UserDTO), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    [ProducesResponseType(typeof(ErrorResponse), 406)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public async Task<ActionResult> Logout([FromBody] LogoutDTO data)
    {
        _logger.LogInformation(LogEvents.INCOMING_REQUEST, "New logout request");
        await _service.Logout(data.GetUserTokenRequest());
        return Ok();
    }

    /// <summary>
    /// This method process security code requests by generating a random string and
    /// sending it via email to the provided address.
    /// </summary>
    /// <param name="data">
    /// The data to be able of sending a security code to the user.
    /// </param>
    /// <response code="200">If the security code has been generated and send successfully</response>
    /// <response code="400">If the received data does not fulfill the requirements</response>
    /// <response code="403">If the user provided an invalid email address (not assignable to any account)</response>
    /// <response code="406">If the user account does not have a valid username</response>
    /// <response code="500">If any other kind of server error occur</response>
    [HttpPost("resetPassword")]
    [ProducesResponseType(typeof(UserDTO), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    [ProducesResponseType(typeof(ErrorResponse), 406)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public async Task<ActionResult> GetSecurityCode([FromBody] GetCodeDTO data)
    {
        _logger.LogInformation(LogEvents.INCOMING_REQUEST, "New request to generate security code");
        await _service.SecurityCode(data.GetUserResetRequest());
        return Ok();
    }

    /// <summary>
    /// This method process reset password requests by analysing the provided security code and
    /// generating a new random password send via email to the user.
    /// </summary>
    /// <param name="data">
    /// The data to generate a new password.
    /// </param>
    /// <response code="200">If the new password has been generated and send successfully</response>
    /// <response code="400">If the received data does not fulfill the requirements</response>
    /// <response code="403">
    /// If the user provided an invalid email address (not assignable to any account).
    /// If the provided security code is invalid or expired.
    /// </response>
    /// <response code="406">If the user account does not have a valid username or security code</response>
    /// <response code="500">If any other kind of server error occur</response>
    [HttpPut("resetPassword")]
    [ProducesResponseType(typeof(UserDTO), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    [ProducesResponseType(typeof(ErrorResponse), 406)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDTO data)
    {
        _logger.LogInformation(LogEvents.INCOMING_REQUEST, "New reset password request");
        await _service.ResetPassword(data.GetUserResetRequest());
        return Ok();
    }

    /// <summary>
    /// This method process any reauthentication request by analysing the provided refresh token and
    /// create new JWT-token.
    /// </summary>
    /// <param name="data">
    /// The data to be able of authenticate the user and generating a new JWT.
    /// </param>
    /// <returns>A session object including all user data stored on the client side</returns>
    /// <response code="200">If the new token has been generated successfully</response>
    /// <response code="400">If the received data does not fulfill the requirements</response>
    /// <response code="403">
    /// If the user provided an invalid email address (not assignable to any account).
    /// If the provided refresh token is invalid or expired.
    /// </response>
    /// <response code="406">If the user account does not have a valid username</response>
    /// <response code="500">If any other kind of server error occur</response>
    [HttpPost("reauthenticate")]
    [ProducesResponseType(typeof(UserDTO), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 403)]
    [ProducesResponseType(typeof(ErrorResponse), 406)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public async Task<ActionResult<UserDTO>> Reauthenticate([FromBody] ReauthenticateDTO data)
    {
        var user = await _service.GenerateToken(data.GetUserTokenRequest());
        var response = new UserDTO(user);
        return Ok(response);
    }
}
