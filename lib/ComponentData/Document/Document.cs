using System.Text.Json.Serialization;
using StudentPortal.ComponentData.Serialization;

namespace StudentPortal.ComponentData;

public class Document
{
    public required string Type { get; set; }

    public ComponentVersion? Version { get; set; }

    public Dictionary<string, Document> Components { get; set; } = [];

    [JsonConverter(typeof(PrimitiveDictionaryConverter))]
    public Dictionary<string, object> Properties { get; set; } = [];

    [JsonConverter(typeof(PrimitiveDictionaryConverter))]
    public Dictionary<string, object> Metadata { get; set; } = [];
}