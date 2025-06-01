using StudentPortal.ComponentData.Abstractions;

namespace StudentPortal.ComponentData.Components;

public record VideoComponent(Guid Id, string Url) : ComponentBase(Id)
{
    public override T Accept<T>(IComponentDataVisitor<T> visitor) => visitor.Visit(this);
}
