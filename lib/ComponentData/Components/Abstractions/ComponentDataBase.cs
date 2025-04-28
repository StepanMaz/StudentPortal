using System.Collections;
using StudentPortal.ComponentData.Abstractions;

namespace StudentPortal.ComponentData.Components;

public abstract record ComponentDataBase : IComponentData
{
    public abstract T Accept<T>(IComponentDataVisitor<T> visitor);
}