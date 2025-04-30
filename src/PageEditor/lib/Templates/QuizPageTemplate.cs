
using System.Collections.Immutable;
using StudentPortal.ComponentData.Abstractions;
using StudentPortal.ComponentData.Components;

namespace StudentPortal.PageEditor.Templates;

public class QuizPageTemplate : IPageTemplate
{
    public IEnumerable<IComponentFactory> Factories => ComponentFactories;

    public IComponentData PageTemplate => new RootComponent(
        new SectionComponent(
            [
                ComponentFactory.MultiAnswerQuestion.CreateInstance()
            ],
            SectionDisplayMode.Pages
        ),
        new QuizControllerComponent()
    );

    public string DisplayName => "Quiz";

    private static IEnumerable<IComponentFactory> ComponentFactories = [
        ComponentFactory.SingleAnswerQuestion,
        ComponentFactory.MultiAnswerQuestion,
        ComponentFactory.Section,
        ComponentFactory.Text,
        ComponentFactory.YoutubeVideo,
        ComponentFactory.Galley
    ];
}