using StudentPortal.ComponentData.Abstractions;

namespace StudentPortal.PageEditor.Templates;

public interface IPageTemplate
{
    public string DisplayName { get; }
    public IEnumerable<IComponentFactory> Factories { get; }
    public IComponentData PageTemplate { get; }
}

public interface IComponentFactory
{
    public abstract string DisplayName { get; }

    public abstract IComponentData CreateInstance();
}