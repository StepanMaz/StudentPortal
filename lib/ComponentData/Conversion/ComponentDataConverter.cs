using System.Reflection;
using StudentPortal.ComponentData.Abstractions;
using StudentPortal.ComponentData.Conversion.Abstractions;

namespace StudentPortal.ComponentData.Conversion;

public class ComponentDataConverter(ITypeResolver resolver) : IDocumentToComponentDataConverter
{
    public IComponentData Convert(Document document)
    {
        return (IComponentData)CreateFromDocument(document);
    }

    public object CreateFromDocument(Document document)
    {
        var type = resolver.Resolve(document.Type);

        var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);

        var IsPOCO = constructors.Any(x => x.GetParameters().Length == 0);

        if (IsPOCO)
        {
            return CreatePOCO(type, document);
        }

        return CreateRecord(type, document);
    }

    private object CreatePOCO(Type type, Document document)
    {
        var @object = Activator.CreateInstance(type)!;

        foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            var name = prop.Name;

            if (document.Properties.TryGetValue(name, out var property))
            {
                prop.SetValue(@object, TypeConverter.ConvertPrimitiveToType(prop.PropertyType, property));
            }
            else if (document.Components.TryGetValue(name, out var component))
            {
                prop.SetValue(@object, CreateFromDocument(component));
            }
            else if (document.ComponentCollections.TryGetValue(name, out var componentCollection))
            {
                var propType = prop.PropertyType;

                if (!propType.TryGetCollectionType(out var elementType)) throw new InvalidOperationException("Target type is not a collection");

                prop.SetValue(@object, CollectionCreationHelper.CreateCollection(propType, componentCollection.Select(x => CreateFromDocument(x))));
            }
        }

        return @object;
    }

    private object CreateRecord(Type type, Document document)
    {
        var constructors = type.GetConstructors();

        if (constructors.Length != 1)
        {
            throw new InvalidOperationException($"Could not determine constructor to create {type.Name} instance");
        }

        var targetConstructor = constructors[0];

        var parameters = targetConstructor.GetParameters();

        var constructorParameters = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            ParameterInfo prop = parameters[i]!;

            if (document.Properties.TryGetValue(prop.Name!, out var property))
            {
                constructorParameters[i] = TypeConverter.ConvertPrimitiveToType(prop.ParameterType, property)!;
            }
            else if (document.Components.TryGetValue(prop.Name!, out var component))
            {
                constructorParameters[i] = CreateFromDocument(component);
            }
            else if (document.ComponentCollections.TryGetValue(prop.Name!, out var componentCollection))
            {
                var propType = prop.ParameterType;

                if (!propType.TryGetCollectionType(out var elementType)) throw new InvalidOperationException("Target type is not a collection");

                constructorParameters[i] = CollectionCreationHelper.CreateCollection(propType, componentCollection.Select(x => CreateFromDocument(x)));
            }
        }

        return Activator.CreateInstance(type, constructorParameters)!;
    }
}
