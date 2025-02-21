using StudentPortal.ComponentData.Components;

namespace StudentPortal.ComponentData.Abstractions;

public interface IComponentData
{
    public T Accept<T>(IComponentDataVisitor<T> visitor);
}

public interface IComponentDataVisitor<T>
{
    public T Visit(MarkdownComponent component);
}