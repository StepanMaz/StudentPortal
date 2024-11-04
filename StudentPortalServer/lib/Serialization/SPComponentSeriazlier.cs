using Dumpify;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using StudentPortalServer.Entities.Page;

namespace StudentPortalServer.Serialization;

public class SPComponentSerializer : SerializerBase<ISPComponent>
{
    private const string TYPE_FIELD = "_t";

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ISPComponent value)
    {
        var writer = context.Writer;

        writer.WriteStartDocument();

        writer.WriteName(TYPE_FIELD);
        writer.WriteString(value.GetType().FullName);

        var properties = value.GetType().GetProperties();
        foreach (var property in properties)
        {
            var propertyValue = property.GetValue(value);
            writer.WriteName(property.Name);
            var serializer = BsonSerializer.LookupSerializer(property.PropertyType);
            serializer.Serialize(context, propertyValue);
        }

        writer.WriteEndDocument();
    }
}