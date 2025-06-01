using StudentPortal.ComponentData.Abstractions;
using StudentPortal.ComponentData.Components;

namespace StudentPortal.PageEditor.Templates;

public class DefaultPageTemplate : IPageTemplate
{
    public string DisplayName => "Material";

    public IEnumerable<IComponentFactory> Factories => ComponentFactories;
    public IComponentData PageTemplate => new RootComponent(Guid.NewGuid(), new SectionComponent(Guid.NewGuid(), [ComponentFactory.Text.CreateInstance()], SectionDisplayMode.Sequential));

    private static IEnumerable<IComponentFactory> ComponentFactories = [
        ComponentFactory.Text,
        ComponentFactory.Section,
        ComponentFactory.SimpleQuestion,
        ComponentFactory.YoutubeVideo,
        ComponentFactory.Galley
    ];
}
