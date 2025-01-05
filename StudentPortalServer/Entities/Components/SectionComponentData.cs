namespace StudentPortalServer.Entities.Page;

public record SectionComponentData(IEnumerable<ISPComponent> Components) : ISPComponent
{
    public T Accept<T>(ISPComponentVisitor<T> visitor) => visitor.Visit(this);
}