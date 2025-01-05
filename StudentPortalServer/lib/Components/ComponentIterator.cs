using StudentPortalServer.Entities.Page;

namespace StudentPortalServer.Components;

public class SPComponentEnumerable : ISPComponentVisitor<IEnumerable<ISPComponent>>
{
    public static readonly SPComponentEnumerable Instance = new();

    public IEnumerable<ISPComponent> Visit(MarkdownComponentData componentData)
    {
        yield return componentData;
    }

    public IEnumerable<ISPComponent> Visit(SectionComponentData componentData)
    {
        yield return componentData;
        foreach (var component in componentData.Components)
        {
            foreach (var subComponent in component.Accept(this))
            {
                yield return subComponent;
            }
        }
    }
}

public static class SPComponentIteratorExtension
{
    public static IEnumerable<ISPComponent> Enumerate(this ISPComponent root)
    {
        return root.Accept(SPComponentEnumerable.Instance);
    }
}