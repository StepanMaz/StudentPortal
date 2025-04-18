using StudentPortal.ComponentData.Abstractions;
using StudentPortal.ComponentData.Components;

namespace StudentPortal.PageEditor.Templates;

public class DefaultPageTemplate : IPageTemplate
{
    public IEnumerable<IComponentFactory> Factories { get; set; }

    private DefaultPageTemplate()
    {
        List<IComponentFactory> componentFactories = [];

        componentFactories.Add(new DefaultComponentFactory("Section", () => new SectionComponent([], SectionDisplayMode.Sequential)));
        componentFactories.Add(new DefaultComponentFactory("Text", () => new MarkdownComponent("")));
        componentFactories.Add(new DefaultComponentFactory("Test Question", () =>
        {
            var variantYes = new VarianceQuestionComponent.VarianceAnswer(Guid.NewGuid(), "Yes");
            var variantNo = new VarianceQuestionComponent.VarianceAnswer(Guid.NewGuid(), "No");
            return new VarianceQuestionComponent(Guid.NewGuid(), "New Question?", Variants: [variantYes, variantNo], CorrectAnswersIds: [variantYes.Id], false);
        }));
        componentFactories.Add(new DefaultComponentFactory("Open Question", () => new OpenAnswerQuestionComponent(Guid.NewGuid(), "What's on your mind?")));
        componentFactories.Add(new DefaultComponentFactory("YouTube Video", () => new VideoComponent("")));
        componentFactories.Add(new DefaultComponentFactory("Image Gallery", () => new GalleyComponent([""])));

        Factories = componentFactories;
    }

    public readonly static DefaultPageTemplate Instance = new DefaultPageTemplate();


    record DefaultComponentFactory(string DisplayName, Func<IComponentData> Instantiate) : IComponentFactory
    {
        public IComponentData CreateInstance() => Instantiate();
    }
}
