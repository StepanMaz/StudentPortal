using StudentPortal.ComponentData.Abstractions;
using StudentPortal.ComponentData.Components;

namespace StudentPortal.PageEditor.Templates;

public class QuizPageTemplate : IPageTemplate
{
    public IEnumerable<IComponentFactory> Factories => ComponentFactories;

    public IComponentData PageTemplate => new RootComponent(Guid.NewGuid(), 
        new SectionComponent(Guid.NewGuid(), 
            [
                ComponentFactory.SingleAnswerQuestion.CreateInstance()
            ],
            SectionDisplayMode.Pages
        ),
        new QuizControllerComponent(Guid.NewGuid())
    );

    public string DisplayName => "Quiz";

    private static IEnumerable<IComponentFactory> ComponentFactories = [
        ComponentFactory.MultiAnswerQuestion,
        ComponentFactory.SingleAnswerQuestion,
        ComponentFactory.OpenQuestion,
        ComponentFactory.Section,
        ComponentFactory.Text,
        ComponentFactory.YoutubeVideo,
        ComponentFactory.Galley
    ];
}