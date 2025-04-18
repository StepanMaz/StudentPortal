using StudentPortal.ComponentData.Abstractions;

namespace StudentPortal.ComponentData.Components;

public record VideoComponent(string Url) : ComponentDataBase
{
    public override T Accept<T>(IComponentDataVisitor<T> visitor) => visitor.Visit(this);
}
