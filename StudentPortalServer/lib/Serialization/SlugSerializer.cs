using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using StudentPortalServer.Entities.Page;

namespace StudentPortalServer.Serialization;

public class SlugSerializer : SerializerBase<Slug>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Slug value)
    {
        var writer = context.Writer;
        writer.WriteString(value);
    }

    public override Slug Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var reader = context.Reader;
        return reader.ReadString();
    }
}