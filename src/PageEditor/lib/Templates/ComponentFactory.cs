using System.Collections.Immutable;
using StudentPortal.ComponentData.Abstractions;
using StudentPortal.ComponentData.Components;

namespace StudentPortal.PageEditor.Templates;

public record ComponentFactory(string DisplayName, Func<IComponentData> Instantiate) : IComponentFactory
{
    public IComponentData CreateInstance() => Instantiate();

    public static readonly ComponentFactory Section = new ComponentFactory("Section", () => new SectionComponent([], SectionDisplayMode.Sequential));
    public static readonly ComponentFactory Text = new ComponentFactory("Text", () => new MarkdownComponent(""));
    public static readonly ComponentFactory YoutubeVideo = new ComponentFactory("Youtube video", () => new VideoComponent(string.Empty));
    public static readonly ComponentFactory Galley = new ComponentFactory("Gallery", () => new GalleyComponent([]));
    public static readonly ComponentFactory MultiAnswerQuestion = new ComponentFactory("Quiz Question (Single)", () =>
    {
        ImmutableList<VarianceQuestionComponent.VarianceAnswer> variants = [new(Guid.NewGuid(), ""), new(Guid.NewGuid(), ""), new(Guid.NewGuid(), "")];
        return new VarianceQuestionComponent(Guid.NewGuid(), "", variants, [variants[0].Id], true);
    });
    public static readonly ComponentFactory SingleAnswerQuestion = new ComponentFactory("Quiz Question (Multiple)", () =>
    {
        Random random = new Random();
        ImmutableList<VarianceQuestionComponent.VarianceAnswer> variants = [new(Guid.NewGuid(), ""), new(Guid.NewGuid(), ""), new(Guid.NewGuid(), "")];
        return new VarianceQuestionComponent(Guid.NewGuid(), "", variants, variants.OrderBy(_ => random.Next()).Select(x => x.Id).Take(2).ToImmutableHashSet(), true);
    });
    public static readonly ComponentFactory SimpleQuestion = new ComponentFactory("Simple Question", () =>
    {
        Random random = new Random();
        ImmutableList<VarianceQuestionComponent.VarianceAnswer> variants = [new(Guid.NewGuid(), "Yes"), new(Guid.NewGuid(), "No"), new(Guid.NewGuid(), "Maybe")];
        return new VarianceQuestionComponent(Guid.NewGuid(), "To be or not to be?", variants, [variants[random.Next(variants.Count)].Id], true);
    });
}
