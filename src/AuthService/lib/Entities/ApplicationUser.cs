
using Microsoft.AspNetCore.Identity;

namespace StudentPortal.AuthService.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}