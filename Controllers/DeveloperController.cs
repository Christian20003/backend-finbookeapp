using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Wrapper;
using FinBookeAPI.Services.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinBookeAPI.Controllers;

[ApiController]
[Route("fill")]
public class DeveloperController(IAuthenticationService service) : ControllerBase
{
    private readonly IAuthenticationService _service = service;

    [HttpPost]
    [Route("createAccount")]
    public async Task<ActionResult> CreateAccount()
    {
        var user = new UserRegister
        {
            Email = "lindner@gmail.com",
            Name = "Lindner",
            Password = "1aB$5555555",
        };
        var result = await _service.Register(user);
        return Ok(result);
    }
}
