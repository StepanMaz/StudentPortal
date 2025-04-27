using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using StudentPortal.ComponentData;

namespace StudentPortal.PageStorage.Entities;

#nullable disable

public class Page
{
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public required Guid OwnerId { get; set; }

    public required Document Content { get; set; }
    public required string Name { get; set; }
    public string Key { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now;

    public Dictionary<string, object> Metadata { get; set; } = [];
}