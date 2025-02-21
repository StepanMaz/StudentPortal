using System.Text.Json;
using System.Text.Json.Serialization;

namespace StudentPortal.ComponentData.Serialization;

public class PrimitiveDictionaryConverter : JsonConverter<Dictionary<string, object>>
{
    public override Dictionary<string, object> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dictionary = new Dictionary<string, object>();

        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected StartObject");

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return dictionary;

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException("Expected PropertyName");

            string propertyName = reader.GetString()!;
            reader.Read();

            object value = reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString()!,
                JsonTokenType.Number => reader.TryGetInt64(out long l) ? l : reader.GetDouble(),
                JsonTokenType.True => true,
                JsonTokenType.False => false,
                JsonTokenType.Null => null!,
                _ => throw new JsonException($"Unsupported JSON token: {reader.TokenType}")
            };

            dictionary[propertyName] = value;
        }

        throw new JsonException("Unexpected end of JSON.");
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, object> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        foreach (var kvp in value)
        {
            writer.WritePropertyName(kvp.Key);

            switch (kvp.Value)
            {
                case string s:
                    writer.WriteStringValue(s);
                    break;
                case long l:
                    writer.WriteNumberValue(l);
                    break;
                case double d:
                    writer.WriteNumberValue(d);
                    break;
                case bool b:
                    writer.WriteBooleanValue(b);
                    break;
                case null:
                    writer.WriteNullValue();
                    break;
                default:
                    throw new JsonException($"Unsupported value type: {kvp.Value?.GetType()}");
            }
        }
        writer.WriteEndObject();
    }
}
