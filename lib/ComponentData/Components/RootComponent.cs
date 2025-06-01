
using StudentPortal.ComponentData.Abstractions;

namespace StudentPortal.ComponentData.Components;

public record RootComponent(Guid Id, IComponentData Content, IComponentData? Footer = null) : ComponentBase(Id)
{
    public override T Accept<T>(IComponentDataVisitor<T> visitor) => visitor.Visit(this);
}