using FinBookeAPI.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinBookeAPI.Controllers;

[ApiController]
[Route("fill")]
public class DeveloperController(UserManager<UserDatabase> userManager) : ControllerBase
{
    private readonly UserManager<UserDatabase> _userManager = userManager;

    [HttpPost]
    [Route("createAccount")]
    public async Task<ActionResult> CreateAccount()
    {
        var user = new UserDatabase
        {
            Id = "c0e4a9ed-1e90-4b20-916a-4c8a3f7a54aa",
            UserName = "FunnyGuy",
            Email = "funny.guy@gmail.com",
        };
        var result = await _userManager.CreateAsync(user);
        var result2 = await _userManager.AddPasswordAsync(user, "12345");
        if (result.Succeeded && result2.Succeeded)
        {
            return Ok();
        }
        return StatusCode(500);
    }
}
