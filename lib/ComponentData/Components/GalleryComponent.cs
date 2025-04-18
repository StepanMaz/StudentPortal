using System.Collections.Immutable;
using StudentPortal.ComponentData.Abstractions;

namespace StudentPortal.ComponentData.Components;

public record GalleyComponent(ImmutableList<string> Links) : ComponentDataBase
{
    public override T Accept<T>(IComponentDataVisitor<T> visitor) => visitor.Visit(this);
}
