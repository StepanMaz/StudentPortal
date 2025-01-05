using MongoDB.Bson.Serialization.Attributes;

namespace StudentPortalServer.Entities.Page;

public interface ISPComponent 
{
    public T Accept<T>(ISPComponentVisitor<T> visitor);
}

public interface ISPComponentVisitor<T>
{
    public T Visit(MarkdownComponentData componentData);
    public T Visit(SectionComponentData componentData);
}