using StudentPortal.ComponentData.Abstractions;

namespace StudentPortal.PageEditor.Templates;

public interface IComponentFactory
{
    public abstract string DisplayName { get; }

    public abstract IComponentData CreateInstance();
}