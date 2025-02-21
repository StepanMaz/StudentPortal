using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using StudentPortal.ComponentData;
using StudentPortal.PageStorage.Serialization;

namespace StudentPortal.PageStorage.Entities;

#nullable disable

public class Page
{
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; } = Guid.NewGuid();

    public required Document Content { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now;

    public Dictionary<string, object> Metadata { get; set; } = [];
}