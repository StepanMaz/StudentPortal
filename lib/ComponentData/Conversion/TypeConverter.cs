using System.Collections;
using System.Text.Json;

namespace StudentPortal.ComponentData.Conversion;

public static class TypeConverter
{
    public static object? ConvertToJsonPrimitive(object @object)
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
        return result;
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
        if (!typeof(IEnumerable).IsAssignableFrom(type))
        {
            throw new InvalidOperationException("Target type should be a collection");
        }

        object target = Activator.CreateInstance(type);

        var addMethod = type.GetMethod("Add");

        if (addMethod is null)
        {
            throw new InvalidOperationException("Target collection type does not have an Add method");
        }

        Type elementType = type.GetInterfaces()
           .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
           .Select(i => i.GetGenericArguments()[0])
           .FirstOrDefault()!;

        foreach (var item in objects)
        {
            addMethod.Invoke(target, [ConvertPrimitiveToType(elementType, item)]);
        }

        return target;
    }
}