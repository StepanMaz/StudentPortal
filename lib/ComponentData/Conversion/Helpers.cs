using System.Collections;
using System.Reflection;

namespace StudentPortal.ComponentData.Conversion;

internal static class ComponentTypeHelperExtensions
{
    public static ComponentVersion? GetComponentVersion(this Type type)
    {
        var attr = type.GetCustomAttribute<ComponentVersionAttribute>();

        return attr?.Version;
    }

    public static bool TryGetCollectionType(this Type collectionType, out Type type)
    {
        if (collectionType.IsAssignableTo(typeof(IEnumerable)))
        {
            var elementType = collectionType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(i => i.GetGenericArguments().First())
                .FirstOrDefault();

            if (elementType is not null)
            {
                type = elementType;
                return true;
            }
        }

        type = null!;
        return false;
    }

}

public static class TypesHelper
{
    public static object CreateCollection(Type collectionType, IEnumerable values)
    {
        if (!typeof(IEnumerable).IsAssignableFrom(collectionType))
        {
            throw new InvalidOperationException("Target type should be a collection");
        }

        if (collectionType.IsArray)
        {
            if (!collectionType.TryGetCollectionType(out var elementType)) throw new InvalidOperationException("Could not determine collection type");

            var items = values.Cast<object>().ToArray();
            var array = Array.CreateInstance(elementType, items.Length);
            items.CopyTo(array, 0);

            return array;
        }

        var target = Activator.CreateInstance(collectionType);

        if (target is null)
        {
            throw new InvalidOperationException("Target type could not be constructed");
        }

        var addMethod = collectionType.GetMethod("Add");

        if (addMethod is null)
        {
            throw new InvalidOperationException("Target collection type does not have an Add method");
        }


        foreach (var item in values)
        {
            addMethod.Invoke(target, [item]);
        }

        return target;
    }
}