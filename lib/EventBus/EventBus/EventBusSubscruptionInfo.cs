using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace StudentPortal.EventBus;

public class EventBusSubscriptionInfo
{
    public JsonSerializerOptions JsonSerializerOptions { get; set; } = DefaultSerializerOptions;
    public Dictionary<string, Type> EventTypes { get; } = new();

    internal static JsonSerializerOptions DefaultSerializerOptions => new()
    {
        TypeInfoResolver = JsonSerializer.IsReflectionEnabledByDefault ? CreateDefaultTypeResolver() : JsonTypeInfoResolver.Combine()
    };

    private static IJsonTypeInfoResolver CreateDefaultTypeResolver()
        => new DefaultJsonTypeInfoResolver();
}