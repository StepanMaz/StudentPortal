using Microsoft.AspNetCore.Identity;
using StudentPortal.AuthService.DB;

public class DataSeeder(RoleManager<IdentityRole> roleManager)
{
    const string TEACHER_ROLE = "Teacher";
    const string STUDENT_ROLE = "Student";

    public async Task SeedAsync()
    {
        if (!await roleManager.RoleExistsAsync(TEACHER_ROLE))
        {
            await roleManager.CreateAsync(new IdentityRole(TEACHER_ROLE));
        }

        if (!await roleManager.RoleExistsAsync(STUDENT_ROLE))
        {
            await roleManager.CreateAsync(new IdentityRole(STUDENT_ROLE));
        }
    }
}
