using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudentPortal.AuthService.Entities;

namespace StudentPortal.AuthService.Controllers;

[ApiController, Route("/")]
public class AuthController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IOptions<JWTConfig> jwtConfig,
    ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO model)
    {
        logger.LogInformation("Trying to register new user: {Email}", model.Email);
        var user = new ApplicationUser()
        {
            UserName = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
        };

        IdentityResult result;
        try
        {
            result = await userManager.CreateAsync(user, model.Password);
        }
        catch
        {
            logger.LogError("Error registering a user {Email}", user.Email);
            return BadRequest();
        }

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, model.Role);

            logger.LogInformation("A new user has been registered: Username: {Username}, FirstName: {FirstName}, LastName: {LastName}, Email: {Email}",
                user.UserName,
                user.FirstName,
                user.LastName,
                user.Email);

            IList<string> roles = [model.Role];

            var token = IssueToken(user, roles);

            SetAuthCookie(token);

            return Ok(new UserDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = roles,
                JwtToken = token 
            });
        }

        logger.LogInformation("Could not register new user. Username: {Username}, FirstName {FirstName}, LastName: {LastName}. Errors: {Errors}", user.UserName, user.FirstName, user.LastName, JsonSerializer.Serialize(result.Errors));

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO model)
    {
        var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

        if (!result.Succeeded)
        {
            logger.LogInformation("User login failed. User: {Username}", model.Email);
            return Unauthorized();
        }

        var user = (await userManager.FindByNameAsync(model.Email))!;

        logger.LogInformation("User login successful. User: {Username}", model.Email);

        var roles = await userManager.GetRolesAsync(user);

        var token = IssueToken(user, roles);

        SetAuthCookie(token);

        return Ok(new UserDTO()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            Roles = roles,
            JwtToken = token
        });
    }

    private void SetAuthCookie(string token)
    {
        HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions() { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict, Expires = DateTimeOffset.UtcNow.AddMonths(1) });
    }

    private string IssueToken(ApplicationUser user, IList<string> roles)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Value.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(ClaimTypes.NameIdentifier, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!)
        ];

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: jwtConfig.Value.Issuer,
            audience: jwtConfig.Value.Audience,
            claims,
            expires: DateTime.Now.AddMonths(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class RegisterDTO
{
#nullable disable
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
#nullable restore
}

public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
{
    public RegisterDTOValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Password).Must(value => value == "Student" || value == "Teacher");
    }
}

public class LoginDTO
{
#nullable disable
    public string Email { get; set; }
    public string Password { get; set; }
#nullable restore
}

public class LoginDTOValidator : AbstractValidator<LoginDTO>
{
    public LoginDTOValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class UserDTO
{
    public required string Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required IEnumerable<string> Roles { get; set; }
    public required string JwtToken { get; set; }
}