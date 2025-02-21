using System.Text.Json;
using System.Text.Json.Serialization;

namespace StudentPortal.ComponentData.Serialization;

public class ComponentVersionJsonConverter : JsonConverter<ComponentVersion>
{
    public override ComponentVersion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
        {
            return ComponentVersion.Empty;
        }

        var versionString = JsonSerializer.Deserialize<string>(ref reader, options);

        if (ComponentVersion.TryParse(versionString, null, out var version))
        {
            return version;
        }

        return ComponentVersion.Empty;
    }

    public override void Write(Utf8JsonWriter writer, ComponentVersion value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.ToString(), options);
    }
}
