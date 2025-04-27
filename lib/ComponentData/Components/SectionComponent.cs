using System.Collections.Immutable;
using StudentPortal.ComponentData.Abstractions;

namespace StudentPortal.ComponentData.Components;

public enum SectionDisplayMode 
{
    Pages,
    Sequential
}

public record SectionComponent(ImmutableList<IComponentData> Components, SectionDisplayMode DisplayMode) : ComponentDataBase
{
    public override T Accept<T>(IComponentDataVisitor<T> visitor) => visitor.Visit(this);
}