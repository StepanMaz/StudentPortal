using StudentPortal.ComponentData.Abstractions;
using StudentPortal.ComponentData.Conversion.Abstractions;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace StudentPortal.ComponentData.Conversion;

public class DocumentConverter : IComponentDataToDocumentConverter<IComponentData>
{
    public Document Convert(IComponentData component)
    {
        return Convert(component as object);
    }

    public Document Convert(object component)
    {
        var componentType = component.GetType();

        var objectProperties = componentType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        var componentProperties = objectProperties.Where(PropertyTypeIs(typeof(IComponentData)));
        var componentCollectionsProperties = objectProperties.Where(PropertyTypeIsCollectionOf(typeof(IComponentData)));
        var other = objectProperties.Where(p => !componentProperties.Contains(p) && !componentCollectionsProperties.Contains(p));

        var components = new Dictionary<string, object>();

        return new Document()
        {
            Type = componentType.Name,
            Version = componentType.GetComponentVersion(),
            Components = GetPrimitiveComponents(componentProperties, component),
            ComponentCollections = GetPrimitiveComponentLists(componentCollectionsProperties, component),
            Properties = GetPrimitiveProperties(other, component)
        };

        static Func<PropertyInfo, bool> PropertyTypeIs(Type type)
        {
            return (PropertyInfo p) =>
            {
                var propertyType = p.PropertyType;
                return propertyType.IsAssignableTo(type);
            };
        }

        static Func<PropertyInfo, bool> PropertyTypeIsCollectionOf(Type type)
        {
            return (PropertyInfo p) =>
            {
                var propertyType = p.PropertyType;

                if (!propertyType.TryGetCollectionType(out var elementType)) return false;

                return elementType.IsAssignableTo(type);
            };
        }
    }

    private Dictionary<string, Document> GetPrimitiveComponents(IEnumerable<PropertyInfo> objectProperties, object @object)
    {
        Dictionary<string, Document> components = [];

        foreach (var prop in objectProperties)
        {
            Debug.Assert(prop.PropertyType.IsAssignableTo(typeof(IComponentData)));

            var name = prop.Name;

            var componentValue = prop.GetValue(@object) as IComponentData;

            if (componentValue is null) continue;

            var value = Convert(componentValue);

            components.Add(name, value);
        }

        return components;
    }

    private Dictionary<string, Document[]> GetPrimitiveComponentLists(IEnumerable<PropertyInfo> objectProperties, object @object)
    {
        var componentCollections = new Dictionary<string, Document[]>();

        foreach (var prop in objectProperties)
        {
            {
                Debug.Assert(prop.PropertyType.TryGetCollectionType(out var elementType));
                Debug.Assert(elementType.IsAssignableTo(typeof(IComponentData)));
            }

            var name = prop.Name;

            var collection = prop.GetValue(@object) as IEnumerable;
            var componentCollection = collection?.Cast<IComponentData>();

            if (componentCollection is null) continue;

            var value = componentCollection.Select(Convert).ToArray();

            componentCollections.Add(name, value);
        }

        return componentCollections;
    }

    private Dictionary<string, object> GetPrimitiveProperties(IEnumerable<PropertyInfo> objectProperties, object @object)
    {
        var props = new Dictionary<string, object>();

        foreach (var prop in objectProperties)
        {
            var name = prop.Name;
            var value = prop.GetValue(@object);

            var convertedValue = TypeConverter.ConvertToJsonPrimitive(value);

            if (convertedValue is null) continue;

            props.Add(name, convertedValue);
        }

        return props;
    }
}