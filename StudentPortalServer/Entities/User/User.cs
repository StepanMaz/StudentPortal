using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudentPortalServer.Entities;

public class User
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Role Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string PasswordHash { get; set; } = null!;
    public string Salt { get; set; } = null!;
}