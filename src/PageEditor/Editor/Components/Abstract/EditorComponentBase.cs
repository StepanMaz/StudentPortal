using Microsoft.AspNetCore.Components;
using StudentPortal.ComponentData.Abstractions;

namespace StudentPortal.PageEditor.Components;

public delegate void PropagateChanges(IComponentData component);

public abstract class EditorComponentBase<TComponent> : ComponentBase where TComponent : IComponentData
{
    [Parameter, EditorRequired]
    public required TComponent Component { get; set; }

    [CascadingParameter]
    public required PropagateChanges PropagateChanges { get; set; }
}