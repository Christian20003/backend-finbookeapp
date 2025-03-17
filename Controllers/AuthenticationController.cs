using FinBookeAPI.DTO.Authentication;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
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

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO data)
    {
        _logger.LogInformation(LogEvents.OBJECT_INVALID, "New login request");
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _service.Login(data.GetUserLogin());
            var response = new UserDTO(result);
            return Ok(response);
        }
        catch (AuthenticationException exception)
        {
            _logger.LogWarning(
                LogEvents.OPERATION_FAILED,
                exception,
                "Something went wrong during login"
            );
            /* switch (exception.Code)
            {
                case ErrorCodes.INVALID_CREDENTIALS:
                {
                    ModelState.AddModelError("Message", "Invalid user account credentials");
                    return Unauthorized(ModelState);
                }
                case ErrorCodes.ACCESS_DENIED:
                {
                    ModelState.AddModelError(
                        "Message",
                        "You are currently been locked out temporally"
                    );
                    return StatusCode(423, ModelState);
                }
                default:
                {
                    ModelState.AddModelError("Message", "An internal server error occurred");
                    return StatusCode(500, ModelState);
                }
            } */
            return BadRequest();
        }
        catch (Exception exception)
        {
            _logger.LogError(LogEvents.OPERATION_FAILED, exception, "An unexpected error occurred");
            ModelState.AddModelError("Message", "An internal server error occurred");
            return StatusCode(500, ModelState);
        }
    }

    [HttpPost("signup")]
    public async Task<ActionResult<UserClient>> UserRegistration(UserRegister userRegData)
    {
        if (userRegData is null)
        {
            return BadRequest();
        }
        var user = await _service.Register(userRegData);
        return CreatedAtAction("Registration successful", user);
    }
}
