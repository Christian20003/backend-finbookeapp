using FinBookeAPI.DTO.Authentication;
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

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO data)
    {
        _logger.LogInformation(LogEvents.INCOMING_REQUEST, "New login request");
        var result = await _service.Login(data.GetUserLogin());
        var response = new UserDTO(result);
        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> UserRegistration([FromBody] RegisterDTO userRegData)
    {
        var user = await _service.Register(userRegData.GetUserRegister());
        var response = new UserDTO(user);
        return CreatedAtAction("Registration successful", response);
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout([FromBody] LogoutDTO data)
    {
        _logger.LogInformation(LogEvents.INCOMING_REQUEST, "New logout request");
        await _service.Logout(data.GetUserTokenRequest());
        return Ok();
    }

    [HttpPost("resetPassword")]
    public async Task<ActionResult> GetSecurityCode([FromBody] GetCodeDTO data)
    {
        _logger.LogInformation(LogEvents.INCOMING_REQUEST, "New request to generate security code");
        await _service.SecurityCode(data.GetUserResetRequest());
        return Ok();
    }

    [HttpPut("resetPassword")]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDTO data)
    {
        _logger.LogInformation(LogEvents.INCOMING_REQUEST, "New reset password request");
        await _service.ResetPassword(data.GetUserResetRequest());
        return Ok();
    }

    [HttpPost("reauthenticate")]
    public async Task<ActionResult<UserDTO>> Reauthenticate([FromBody] ReauthenticateDTO data)
    {
        var user = await _service.GenerateToken(data.GetUserTokenRequest());
        var response = new UserDTO(user);
        return Ok(response);
    }
}
