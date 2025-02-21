using System.Reflection;
using StudentPortal.ComponentData.Abstractions;
using StudentPortal.ComponentData.Conversion.Abstractions;

namespace StudentPortal.ComponentData.Conversion;

public class TypeRegistry : ITypeResolver
{
    private Dictionary<string, Type> _types = [];

    public void Register<T>()
    {
        Register(typeof(T));
    }

    public void Register(Type type)
    {
        _types.Add(type.Name, type);
    }

    public Type Resolve(string input)
    {
        return _types[input];
    }

    public static TypeRegistry AssemblyBased
    {
        get
        {
            var assembly = Assembly.GetAssembly(typeof(TypeRegistry))!;

            var types = assembly
                .GetTypes()
                .Where(type => typeof(IComponentData).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            var registry = new TypeRegistry();

            foreach (var type in types)
            {
                registry.Register(type);
            }

            return registry;
        }
    }
}