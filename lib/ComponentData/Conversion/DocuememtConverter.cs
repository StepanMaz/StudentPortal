using StudentPortal.ComponentData.Abstractions;
using StudentPortal.ComponentData.Conversion.Abstractions;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;

namespace StudentPortal.ComponentData.Conversion;

public class DocumentConverter : IComponentDataToDocumentConverter<IComponentData>
{
    public Document Convert(IComponentData component)
    {
        var componentType = component.GetType();

        var objectProperties = componentType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        return new Document()
        {
            Type = componentType.Name,
            Version = componentType.GetComponentVersion(),
            Components = GetPrimitiveComponents(objectProperties, component),
            Properties = GetPrimitiveProperties(objectProperties, component)
        };
    }

    private Dictionary<string, Document> GetPrimitiveComponents(PropertyInfo[] objectProperties, object @object)
    {
        var subComponents = objectProperties.Where(pi => pi.PropertyType.IsAssignableTo(typeof(IComponentData))).ToList();

        if (subComponents.Count == 0)
        {
            return [];
        }

        var pairs = subComponents.Select(pi => KeyValuePair.Create(pi.Name, (IComponentData)pi.GetValue(@object)!)).Where(kvp => kvp.Value != null);

        return pairs.ToDictionary(x => x.Key, x => Convert(x.Value));
    }

    private Dictionary<string, object> GetPrimitiveProperties(PropertyInfo[] objectProperties, object @object)
    {
        var properties = objectProperties.Where(pi => !pi.PropertyType.IsAssignableTo(typeof(IComponentData))).ToList();

        if (properties.Count == 0)
        {
            return [];
        }

        var pairs = properties.Select(pi => KeyValuePair.Create(pi.Name, pi.GetValue(@object))!).Where(kvp => kvp.Value != null);

        return pairs.ToDictionary(x => x.Key, x => TypeConverter.ConvertToJsonPrimitive(x.Value)!);
    }
}

internal static class ComponentTypeHelperExtensions
{
    public static ComponentVersion? GetComponentVersion(this Type type)
    {
        var attr = type.GetCustomAttribute<ComponentVersionAttribute>();

        return attr?.Version;
    }
}