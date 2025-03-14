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

[ApiController, Route("[controller]")]
public class AuthController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IOptions<JWTConfig> jwtConfig,
    ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponseDTO>> Register([FromBody] RegisterDTO model)
    {
        var user = new ApplicationUser()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, model.Role);

            logger.LogInformation("A new user has been registered: Username: {Username}, FirstName: {FirstName}, LastName: {LastName}, Email: {Email}",
                user.UserName,
                user.FirstName,
                user.LastName,
                user.Email);


            return Ok(new RegisterResponseDTO()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                JwtToken = await IssueToken(user)
            });
        }

        logger.LogInformation("Could not register new user. Username: {Username}, FirstName {FirstName}, LastName: {LastName}. Errors: {Errors}", user.UserName, user.FirstName, user.LastName, JsonSerializer.Serialize(result.Errors));

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO model)
    {
        var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

        if (!result.Succeeded)
        {
            logger.LogInformation("User login failed. User: {Username}", model.Username);
            return Unauthorized();
        }

        var user = (await userManager.FindByNameAsync(model.Username))!;

        logger.LogInformation("User login successful. User: {Username}", model.Username);

        return Ok(new LoginResponseDTO()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            JwtToken = await IssueToken(user)
        });
    }

    private async Task<string> IssueToken(ApplicationUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Value.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(ClaimTypes.NameIdentifier, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!)
        ];

        var roles = await userManager.GetRolesAsync(user);

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
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class LoginDTO
{
#nullable disable
    public string Username { get; set; }
    public string Password { get; set; }
#nullable restore
}

public class LoginDTOValidator : AbstractValidator<LoginDTO>
{
    public LoginDTOValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class RegisterResponseDTO
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string JwtToken { get; set; }
}

public class LoginResponseDTO
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string JwtToken { get; set; }
}