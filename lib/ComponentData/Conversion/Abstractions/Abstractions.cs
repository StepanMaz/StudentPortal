using StudentPortal.ComponentData.Abstractions;

namespace StudentPortal.ComponentData.Conversion.Abstractions;

public interface IComponentDataToDocumentConverter<T> where T : IComponentData
{
    public Document Convert(T component);
}

public interface IDocumentToComponentDataConverter
{
    public IComponentData Convert(Document document);
}

public interface ITypeResolver
{
    public Type Resolve(string input);
}