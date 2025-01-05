using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using StudentPortalServer.Entities.Page;

namespace StudentPortalServer.Serialization;

public class SPComponentSerializer : SerializerBase<ISPComponent>
{
    static SPComponentSerializer()
    {
        RegisterClassMap<MarkdownComponentData>("markdown");
        RegisterClassMap<SectionComponentData>("section");
    }

    private const string TYPE_FIELD = "_t";

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ISPComponent value)
    {
        var writer = context.Writer;

        writer.WriteStartDocument();

        writer.WriteName(TYPE_FIELD);
        writer.WriteString(_nameResolver.ResolveDestination(value.GetType().FullName!));

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

    public override ISPComponent Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        Type spComponentType = GetSPComponentType(context);
        if (IsPOCO(spComponentType))
            return (ISPComponent)CreatePOCOInstance(spComponentType, context);
        return (ISPComponent)CreateRecordInstance(spComponentType, context);
    }

    private bool IsPOCO(Type type)
    {
        return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
        .Any(x => x.GetParameters().Length == 0);
    }

    private object CreatePOCOInstance(Type type, BsonDeserializationContext context)
    {
        var reader = context.Reader;
        var instance = Activator.CreateInstance(type);
        reader.ReadStartDocument();

        while (reader.ReadBsonType() != BsonType.EndOfDocument)
        {
            var fieldName = reader.ReadName();
            var propertyInfo = type.GetProperty(fieldName);
            if (propertyInfo != null)
            {
                var propertySerializer = BsonSerializer.LookupSerializer(propertyInfo.PropertyType);
                var propertyValue = propertySerializer.Deserialize(context);
                propertyInfo.SetValue(instance, propertyValue);
            }
            else
                reader.SkipValue();
        }

        return instance!;
    }

    private object CreateRecordInstance(Type type, BsonDeserializationContext context)
    {
        var reader = context.Reader;
        reader.ReadStartDocument();

        var constructors = type.GetConstructors();
        if (constructors.Length != 1) throw new Exception($"Could not determine constructor to create ${type.Name} instance");

        var targetConstructor = constructors[0];


        Dictionary<string, object> parameters = new();

        while (reader.ReadBsonType() != BsonType.EndOfDocument)
        {
            var fieldName = reader.ReadName();
            var parameter = targetConstructor.GetParameters().FirstOrDefault(x => x.Name == fieldName);
            if (parameter is not null)
            {
                var propertySerializer = BsonSerializer.LookupSerializer(parameter.ParameterType);
                var parameterValue = propertySerializer.Deserialize(context);

                parameters.Add(fieldName, parameterValue);
            }
            else
                reader.SkipValue();
        }

        reader.ReadEndDocument();

        return Activator.CreateInstance(type, targetConstructor.GetParameters().Select(x => parameters[x.Name!]).ToArray()!)!;
    }

    private Type GetSPComponentType(BsonDeserializationContext context)
    {
        var reader = context.Reader;
        var start = reader.GetBookmark();
        reader.ReadStartDocument();

        string? typeName = null;
        while (reader.ReadBsonType() != BsonType.EndOfDocument)
        {
            var fieldName = reader.ReadName();
            if (fieldName == TYPE_FIELD)
                typeName = reader.ReadString();
            else
                reader.SkipValue();
        }

        if (typeName is null)
        {
            reader.ReadEndDocument();
            throw new Exception("Could not parse ISPComponent");
        }

        var resolvedName = _nameResolver.ResolveSource(typeName);
        var type = Type.GetType(resolvedName);

        if (type is null)
            throw new Exception($"Count not find resolved type. Type ${resolvedName}");

        reader.ReturnToBookmark(start);
        return type;
    }

    private static NameResolver _nameResolver = new NameResolver();
    public static void RegisterClassMap<T>(string alias)
    {
        _nameResolver.Register(typeof(T).FullName!, alias);
    }

    [Obsolete("Usage is unstable")]
    public static void RegisterRemap(string dbName, string alias)
    {
        _nameResolver.Register(alias, dbName);
    }
}

internal class NameResolver
{
    private Dictionary<string, string> _sourceToDestination = new();
    private Dictionary<string, string> _destinationToSource = new();

    public void Register(string source, string destination)
    {
        _sourceToDestination.Add(source, destination);
        _destinationToSource.Add(destination, source);
    }

    public string GetSource(string destination)
    {
        return _destinationToSource[destination];
    }

    public string GetDestination(string source)
    {
        return _sourceToDestination[source];
    }

    public bool HasSource(string source)
    {
        return _sourceToDestination.ContainsKey(source);
    }

    public bool HasDestination(string destination)
    {
        return _destinationToSource.ContainsKey(destination);
    }

    public string ResolveSource(string destination)
    {
        if (!HasDestination(destination)) return destination;

        return ResolveSource(GetSource(destination));
    }

    public string ResolveDestination(string source)
    {
        if (!HasSource(source)) return source;

        return ResolveDestination(GetDestination(source));
    }
}