using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace StudentPortal.ComponentData.Conversion;

internal static class ComponentTypeHelperExtensions
{
    public static ComponentVersion? GetComponentVersion(this Type type)
    {
        var attr = type.GetCustomAttribute<ComponentVersionAttribute>();

        return attr?.Version ?? ComponentVersion.Empty;
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

    public static object CastWithType(this IEnumerable values, Type type)
    {
        return typeof(IEnumerable).GetMethod(nameof(Enumerable.Cast))!.MakeGenericMethod(type).Invoke(null, [values])!;
    }
}


public static class CollectionCreationHelper
{
    public static object CreateCollection(Type collection, IEnumerable values)
    {
        object instance;

        if (IsCollection(collection) &&
            (
                TryCreateArray(collection, values, out instance) ||
                TryCreateCollectionFromBuilderAttribute(collection, values, out instance) ||
                TryCreateCollection(collection, values, out instance)
            ))
        {
            return instance;
        }

        throw new Exception($"Failed to create collection of type {collection.Name}");
    }

    public static bool IsCollection(Type type)
    {
        return type.IsAssignableTo(typeof(IEnumerable));
    }

    public static bool TryCreateArray(Type collection, IEnumerable values, out object instance)
    {
        if (!collection.IsArray || !collection.TryGetCollectionType(out var elementType))
        {
            instance = null!;
            return false;
        }

        var items = values.Cast<object>().ToArray();
        var array = Array.CreateInstance(elementType, items.Length);
        items.CopyTo(array, 0);

        instance = array;
        return true;
    }

    public static bool TryCreateCollection(Type collection, IEnumerable values, out object instance)
    {
        if (!collection.TryGetCollectionType(out var elementType))
        {
            instance = null!;
            return false;
        }

        if ((collection.GetGenericTypeDefinition() == typeof(ICollection<>) || collection.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            && TryCreateICollection(typeof(List<>).MakeGenericType(elementType), values, out instance))
        {
            return true;
        }

        if (TryCreateICollection(collection, values, out instance))
        {
            return true;
        }

        instance = null!;
        return false;
    }

    public static bool TryCreateICollection(Type collection, IEnumerable values, out object instance)
    {
        instance = null!;
        if (!collection.TryGetCollectionType(out var elementType) || !collection.IsAssignableTo(typeof(ICollection<>).MakeGenericType(elementType)))
        {
            return false;
        }

        var collectionInstance = Activator.CreateInstance(collection);
        if (collectionInstance is null)
        {
            return false;
        }

        var addMethod = collection.GetMethod(nameof(ICollection<object>.Add));

        if (addMethod is null)
        {
            return false;
        }

        foreach (var item in values)
        {
            addMethod.Invoke(collectionInstance, [item]);
        }

        instance = collectionInstance;
        return true;
    }
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ImmutableArray<>))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ImmutableList<>))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ImmutableHashSet<>))]
    public static bool TryCreateCollectionFromBuilderAttribute(Type collection, IEnumerable values, out object instance)
    {
        instance = null!;
        var attribute = collection.GetCustomAttribute<CollectionBuilderAttribute>();

        if (attribute is null || !collection.TryGetCollectionType(out var elementType))
        {
            return false;
        }

        var builderType = attribute.BuilderType;
        var methodName = attribute.MethodName;

        var candidateMethods = builderType
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == methodName);

        var methodDefinition = candidateMethods.FirstOrDefault(m =>
            {
                var parameters = m.MakeGenericMethod(elementType).GetParameters();
                return m.IsGenericMethodDefinition &&
                    parameters.Length == 1 &&
                    parameters[0].ParameterType.IsAssignableFrom(elementType.MakeArrayType());
            });

        if (methodDefinition == null)
        {
            return false;
        }

        var method = methodDefinition.MakeGenericMethod(elementType);

        if (method == null || !TryCreateArray(elementType.MakeArrayType(), values, out var array))
        {
            return false;
        }


        instance = method.Invoke(null, [array])!;
        return true;
    }
}