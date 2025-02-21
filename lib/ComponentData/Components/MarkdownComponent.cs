using StudentPortal.ComponentData.Abstractions;

namespace StudentPortal.ComponentData.Components;

public record MarkdownComponent(string Content) : IComponentData
{
    public T Accept<T>(IComponentDataVisitor<T> visitor) => visitor.Visit(this);
}
