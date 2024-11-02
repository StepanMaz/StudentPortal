using Dumpify;
using Microsoft.AspNetCore.Mvc;
using StudentPortalServer.Models;
using StudentPortalServer.Services;

namespace StudentPortalServer.Controllers;

[Controller]
[Route("/api/[controller]")]
public class UsersController(UserService userService) : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest data)
    {
        if (!this.ModelState.IsValid)
        {
            return BadRequest(this.ModelState);
        }
        var user = data.ToUser();
        var res = await userService.AddUserAsync(user);

        return Ok(new AuthenticateResponse(res, ""));
    }

    [HttpGet]
    public IActionResult Test()
    {
        return Ok("Text");
    }
}