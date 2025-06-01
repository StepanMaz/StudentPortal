using System.Collections;
using StudentPortal.ComponentData.Abstractions;

namespace StudentPortal.ComponentData.Components;

public abstract record ComponentBase(Guid Id): IComponentData
{
    public abstract T Accept<T>(IComponentDataVisitor<T> visitor);
}