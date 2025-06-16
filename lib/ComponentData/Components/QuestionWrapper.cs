using StudentPortal.ComponentData.Abstractions;

namespace StudentPortal.ComponentData.Components;

public record QuestionWrapperComponent(Guid Id, IComponentData Question) : IComponentData
{
    public T Accept<T>(IComponentDataVisitor<T> visitor) => visitor.Visit(this);
}
