
namespace StudentPortalServer.Models.Components;

public record SectionComponentData(IEnumerable<ISPComponent> Components) : ISPComponent
{
    public T Accept<T>(IComponentVisitor<T> visitor) => visitor.Visit(this);
}