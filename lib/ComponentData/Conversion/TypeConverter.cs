using System.Collections;
using System.Text.Json;

namespace StudentPortal.ComponentData.Conversion;

public static class TypeConverter
{
    public static object? ConvertToJsonPrimitive(object? @object)
    {
        if (@object is null) return null;

        return @object switch
        {
            string or int or long or float or decimal or bool => @object,
            IEnumerable collection => ConvertCollection(collection),
            _ => JsonSerializer.Serialize(@object)
        };
    }

    private static object ConvertCollection(IEnumerable collection)
    {
        var result = new List<object?>();
        foreach (var item in collection)
        {
            result.Add(ConvertToJsonPrimitive(item));
        }
        return result.ToArray();
    }

    public static T ConvertPrimitiveToType<T>(object primitive)
    {
        return (T)ConvertPrimitiveToType(typeof(T), primitive);
    }

    public static object? ConvertPrimitiveToType(Type type, object primitive)
    {
        if (type == primitive.GetType()) return primitive;

        if (primitive is string input)
        {
            return JsonSerializer.Deserialize(input, type);
        }

        if (primitive is IEnumerable objects)
        {
            return ConvertPrimitiveCollection(type, objects);
        }

        return null;
    }

    private static object ConvertPrimitiveCollection(Type type, IEnumerable objects)
    {
        if (!type.TryGetCollectionType(out var elementType)) throw new InvalidOperationException("Target collection element type could not be determined");

        var values = objects.Cast<object>().Select(x => ConvertPrimitiveToType(elementType, x));

        return CollectionCreationHelper.CreateCollection(type, values);
    }
}