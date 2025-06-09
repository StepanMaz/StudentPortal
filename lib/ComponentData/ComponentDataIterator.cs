using StudentPortal.ComponentData.Abstractions;
using StudentPortal.ComponentData.Components;

namespace StudentPortal.ComponentData;

public static class ComponentDataIteratorExtensions
{
    private static ComponentDataIterator _componentIterator = new();

    public static IEnumerable<IComponentData> Components(this IComponentData componentData)
    {
        return componentData.Accept(_componentIterator);
    }
}

public class ComponentDataIterator : IComponentDataVisitor<IEnumerable<IComponentData>>
{
    public IEnumerable<IComponentData> Visit(MarkdownComponent component)
    {
        yield return component;
    }

    public IEnumerable<IComponentData> Visit(RootComponent component)
    {
        yield return component;
        foreach (var item in component.Content.Accept(this))
        {
            yield return item;
        }
    }

    public IEnumerable<IComponentData> Visit(VarianceQuestionComponent component)
    {
        yield return component;
    }

    public IEnumerable<IComponentData> Visit(OpenAnswerQuestionComponent component)
    {
        yield return component;
    }

    public IEnumerable<IComponentData> Visit(SectionComponent component)
    {
        yield return component;

        foreach (var item in component.Components)
        {
            foreach (var subItem in item.Accept(this))
            {
                yield return subItem;
            }
        }
    }

    public IEnumerable<IComponentData> Visit(VideoComponent component)
    {
        yield return component;
    }

    public IEnumerable<IComponentData> Visit(GalleryComponent component)
    {
        yield return component;
    }

    public IEnumerable<IComponentData> Visit(QuizControllerComponent component)
    {
        yield return component;
    }

    public IEnumerable<IComponentData> Visit(QuestionWrapperComponent component)
    {
        yield return component;

        foreach (var item in component.Question.Accept(this))
        {
            yield return item;
        }
    }
}