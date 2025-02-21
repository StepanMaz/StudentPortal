using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using StudentPortal.ComponentData;

namespace StudentPortal.PageStorage.Serialization;

public class ComponentVersionBsonConverter : SerializerBase<ComponentVersion>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ComponentVersion value)
    {
        context.Writer.WriteString(value.ToString());
    }

    public override ComponentVersion Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var versionString = context.Reader.ReadString();

        if (ComponentVersion.TryParse(versionString, null, out var version))
        {
            return version;
        }

        return ComponentVersion.Empty;
    }
}