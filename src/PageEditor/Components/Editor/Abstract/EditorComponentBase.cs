using Microsoft.AspNetCore.Components;
using StudentPortal.ComponentData.Abstractions;
using StudentPortal.PageEditor.Templates;

namespace StudentPortal.PageEditor.Components;

public delegate void PropagateChanges(IComponentData component);

public abstract class EditorComponentBase<TComponent> : ComponentBase where TComponent : IComponentData
{
    [Parameter, EditorRequired]
    public required TComponent Component { get; set; }

    [CascadingParameter]
    protected PropagateChanges? PropagateChanges { get; set; }

    [CascadingParameter]
    protected IPageTemplate Template { get; set; } = DefaultPageTemplate.Instance;

    public void NotifyComponentChanged(TComponent component)
    {
        PropagateChanges?.Invoke(component);
    }
}