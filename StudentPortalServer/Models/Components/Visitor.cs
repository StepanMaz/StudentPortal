namespace StudentPortalServer.Models.Components;

public interface ISPComponent 
{
    public T Accept<T>(IComponentVisitor<T> visitor);
}

public interface IComponentVisitor<T>
{
    public T Visit(MarkdownComponentData componentData);
    public T Visit(SectionComponentData componentData);
}