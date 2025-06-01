using System.Collections.Immutable;
using StudentPortal.ComponentData.Abstractions;

namespace StudentPortal.ComponentData.Components;

public enum SectionDisplayMode 
{
    Pages,
    Sequential
}

public record SectionComponent(Guid Id, ImmutableList<IComponentData> Components, SectionDisplayMode DisplayMode) : ComponentBase(Id)
{
    public override T Accept<T>(IComponentDataVisitor<T> visitor) => visitor.Visit(this);
}