using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Wrapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinBookeAPI.Controllers;

[ApiController]
[Route("fill")]
public class DeveloperController(UserManager<UserDatabase> userManager, IDataProtection protector)
    : ControllerBase
{
    private readonly UserManager<UserDatabase> _userManager = userManager;
    private readonly IDataProtection _protector = protector;

    [HttpPost]
    [Route("createAccount")]
    public async Task<ActionResult> CreateAccount()
    {
        var user = new UserDatabase
        {
            Id = "c0e4a9ed-1e90-4b20-916a-4c8a3f7a54aa",
            UserName = _protector.Protect("FunnyGuy"),
            Email = _protector.Protect("funny.guy@gmail.com"),
        };
        var result = await _userManager.CreateAsync(user, "1aB$5555555");
        Console.WriteLine(result);
        if (result.Succeeded)
        {
            return Ok();
        }
        return StatusCode(500);
    }

    [HttpGet]
    [Route("createAccount")]
    public async Task<UserDatabase> Get()
    {
        return await _userManager.FindByEmailAsync("funny.guy@gmail.com");
    }
}
