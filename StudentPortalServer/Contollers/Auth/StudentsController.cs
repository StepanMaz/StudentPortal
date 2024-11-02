namespace StudentPortal.Controllers.Auth;

using Microsoft.AspNetCore.Mvc;
using StudentPortalServer.Models.DTO.Auth;

[Route("api/auth/student")]
[ApiController]
public class StudentAuthController : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register(StudentRegisterDTO registerDTO)
    {
        return Ok("Registered");
    }

    [HttpPut("login")]
    public IActionResult Login(StudentLoginDTO loginDTO)
    {
        return Ok("Login successful");
    }
}

