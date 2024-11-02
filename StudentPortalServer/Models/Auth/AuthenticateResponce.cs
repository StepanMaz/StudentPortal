using StudentPortalServer.Entities;

namespace StudentPortalServer.Models;

public class AuthenticateResponse
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Token { get; set; }

    public AuthenticateResponse(User user, string token)
    {
        Id = user.Id.ToString();
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
        Role = user.Role.ToString().ToLower();
        Token = token;
    }
}