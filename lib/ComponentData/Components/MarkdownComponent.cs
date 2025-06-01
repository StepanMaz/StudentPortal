using StudentPortal.ComponentData.Abstractions;

namespace StudentPortal.ComponentData.Components;

public record MarkdownComponent(Guid Id, string Content) : ComponentBase(Id)
{
    public override T Accept<T>(IComponentDataVisitor<T> visitor) => visitor.Visit(this);
}
