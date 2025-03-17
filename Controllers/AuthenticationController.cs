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
            return Ok(ModelState);
        }
        var result = await _service.Login(data.GetUserLogin());
        var response = new UserDTO(result);
        return Ok(response);
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
