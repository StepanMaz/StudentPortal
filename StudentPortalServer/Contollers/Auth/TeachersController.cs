namespace StudentPortal.Controllers.Auth;

using StudentPortalServer.Models.DTO.Auth;
using Microsoft.AspNetCore.Mvc;

[Route("api/auth/teacher")]
[ApiController]
public class TeacherAuthController : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register(TeacherRegisterDTO registerDTO)
    {
        return Ok("Registered");
    }

    [HttpPut("login")]
    public IActionResult Login(TeacherLoginDTO loginDTO)
    {
        return Ok("Login successful");
    }
}

