using Microsoft.AspNetCore.Mvc;

namespace StudentPortal.Controllers;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpGet("register")]
    public IActionResult Register()
    {
        return Ok("Registered");
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return Ok("Login successful");
    }
}