namespace StudentPortalServer.Entities.Page;

public record MarkdownComponentData(string Content) : ISPComponent
{
    public T Accept<T>(IComponentVisitor<T> visitor) => visitor.Visit(this);
}