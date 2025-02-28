using StudentPortal.ComponentData.Components;

namespace StudentPortal.ComponentData.Abstractions;

public interface IComponentData
{
    public T Accept<T>(IComponentDataVisitor<T> visitor);
}

public interface IComponentDataVisitor<T>
{
    public T Visit(MarkdownComponent component);
    public T Visit(SingleAnswerQuestionComponent component);
    public T Visit(MultiAnswerQuestionComponent component);
    public T Visit(OpenAnswerQuestionComponent component);
    public T Visit(SectionComponent component);
}