using StudentPortal.ComponentData.Abstractions;

namespace StudentPortal.ComponentData.Components;

public record QuizControllerComponent(Guid Id) : ComponentBase(Id)
{
    public override T Accept<T>(IComponentDataVisitor<T> visitor) => visitor.Visit(this);
}
