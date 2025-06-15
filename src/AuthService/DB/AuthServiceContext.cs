using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentPortal.AuthService.Entities;

namespace StudentPortal.AuthService.DB;

public class AuthServiceContext : IdentityDbContext<ApplicationUser>
{
    public AuthServiceContext(DbContextOptions<AuthServiceContext> options) : base(options)
    {
    }
}