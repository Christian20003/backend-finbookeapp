
using FinBookeAPI.Models.Authentication.Interfaces;
using FinBookeAPI.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_finbookeapp.Controllers;

[ApiController]
[Route("[authorize]")]
[AllowAnonymous]
public class LoginController : ControllerBase
{

    private readonly ILogger<LoginController> _logger;
    private readonly IAuthenticationService _authenticationService;

    public LoginController(IAuthenticationService authenticationService,
                            ILogger<LoginController> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }


    [HttpPost("login")]

    public async Task<ActionResult<IUserClient>> UserLogin(IUserLogin userLoginData)
    {

        if (userLoginData is null)
        {
            return BadRequest();
        }
        var user = await _authenticationService.Login(userLoginData);
        return CreatedAtAction("login successful", user);


    }

    [HttpPost("signup")]
    public async Task<ActionResult<IUserClient>> UserRegistration(IUserRegister userRegData)
    {

        if (userRegData is null)
        {
            return BadRequest();
        }
        var user = await _authenticationService.Register(userRegData);
        return CreatedAtAction("Registration successful", user);


    }

}