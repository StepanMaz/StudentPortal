namespace StudentPortalServer.Models.Components;

public record MarkdownComponentData(string Content) : ISPComponent
{
    public T Accept<T>(IComponentVisitor<T> visitor) => visitor.Visit(this);
}