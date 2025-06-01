using System.Collections.Immutable;
using StudentPortal.ComponentData.Abstractions;

namespace StudentPortal.ComponentData.Components;

public record GalleryComponent(Guid Id, ImmutableList<string> Links) : ComponentBase(Id)
{
    public override T Accept<T>(IComponentDataVisitor<T> visitor) => visitor.Visit(this);
}
