using Dumpify;
using MongoDB.Bson.Serialization;
using StudentPortalServer.Entities.Page;

namespace StudentPortalServer.Serialization;

public class SPComponentSerializer : IBsonSerializer<ISPComponent>
{
    private const string TYPE_FIELD = "_t";
    public Type ValueType => typeof(ISPComponent);

    public ISPComponent Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        throw new NotImplementedException();
    }

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ISPComponent value)
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

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
    {
        throw new NotImplementedException();
    }

    object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        throw new NotImplementedException();
    }

    public static void Setup()
    {
        BsonSerializer.RegisterSerializer(typeof(ISPComponent), new SPComponentSerializer());
    }
}