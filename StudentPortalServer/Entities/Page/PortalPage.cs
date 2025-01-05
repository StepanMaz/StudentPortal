using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using StudentPortalServer.Entities.Page;
using StudentPortalServer.Serialization;

namespace StudentPortalServer.Entities;

public class PortalPage
{
    [BsonId]
    public ObjectId Id { get; set; }

    public required Slug Slug { get; set; }

    public required ISPComponent Content { get; set; }
}