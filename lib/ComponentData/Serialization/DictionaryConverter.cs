using System.Collections;
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

            dictionary[propertyName] = ReadValue(ref reader);
        }

        throw new JsonException("Unexpected end of JSON.");
    }

    private object ReadValue(ref Utf8JsonReader reader)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => reader.GetString()!,
            JsonTokenType.Number => reader.TryGetInt64(out long l) ? l : reader.GetDouble(),
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Null => null!,
            JsonTokenType.StartArray => ReadArray(ref reader),
            _ => throw new JsonException($"Unsupported JSON token: {reader.TokenType}")
        };
    }

    private object ReadArray(ref Utf8JsonReader reader)
    {
        List<object> values = new List<object>();
        while (reader.Read())
        {
            if(reader.TokenType == JsonTokenType.EndArray)
                return values.ToList();

            values.Add(ReadValue(ref reader));
        }

        throw new Exception("Unexpected end of JSON document");
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, object> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        foreach (var kvp in value)
        {
            writer.WritePropertyName(kvp.Key);
            WriteValue(kvp.Value, writer);
        }
        writer.WriteEndObject();
    }

    private void WriteValue(object value, Utf8JsonWriter writer)
    {
        switch (value)
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
            case IEnumerable data:
                WriteCollection(data, writer);
                break;
            default:
                throw new JsonException($"Unsupported value type: {value?.GetType()}");
        }
    }

    private void WriteCollection(IEnumerable values, Utf8JsonWriter writer)
    {
        writer.WriteStartArray();
        foreach (var item in values)
        {
            WriteValue(item, writer);
        }
        writer.WriteEndArray();
    }
}
