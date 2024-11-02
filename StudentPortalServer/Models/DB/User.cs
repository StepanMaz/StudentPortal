using MongoDB.Bson;
using StudentPortalServer.Authorization;

namespace StudentPortalServer.Models.DB;

public class User
{
    public ObjectId Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Role Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string PasswordHash { get; set; } = null!;
}