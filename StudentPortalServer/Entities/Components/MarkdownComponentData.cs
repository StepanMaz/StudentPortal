namespace StudentPortalServer.Entities.Page;

public record MarkdownComponentData(string Content) : ISPComponent
{
    public T Accept<T>(ISPComponentVisitor<T> visitor) => visitor.Visit(this);
}