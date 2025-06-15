using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentPortal.AuthService.Entities;

namespace StudentPortal.AuthService.Controllers;

[ApiController, Route("users")]
public class UserController(
    UserManager<ApplicationUser> userManager,
    ILogger<UserController> logger) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<SimpleUserDTO>> GetUser(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        if (user is null) return NotFound();

        var roles = await userManager.GetRolesAsync(user);

        return new SimpleUserDTO()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Roles = roles
        };
    }
}

public class SimpleUserDTO
{
#nullable disable
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public IEnumerable<string> Roles { get; set; }
#nullable restore
}
