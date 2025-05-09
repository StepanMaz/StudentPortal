
using StudentPortal.ComponentData.Abstractions;

namespace StudentPortal.ComponentData.Components;

public record RootComponent(IComponentData Content, IComponentData? Footer = null) : ComponentDataBase 
{
    public override T Accept<T>(IComponentDataVisitor<T> visitor) => visitor.Visit(this);
}