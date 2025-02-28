using System.Collections.Immutable;
using StudentPortal.ComponentData.Abstractions;
using StudentPortal.ComponentData.Components;

namespace StudentPortal.ComponentData;

public record SectionComponent(IImmutableList<IComponentData> Components) : ComponentDataBase
{
    public override T Accept<T>(IComponentDataVisitor<T> visitor) => visitor.Visit(this);
}