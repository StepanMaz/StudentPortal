using Microsoft.AspNetCore.Components;
using StudentPortal.ComponentData.Abstractions;
using StudentPortal.PageEditor.Templates;

namespace StudentPortal.PageEditor.Components;

public delegate void PropagateChanges(IComponentData component);


public abstract class EditorComponentBase<TComponent> : ComponentBase where TComponent : IComponentData
{
    [Parameter, EditorRequired]
    public required TComponent Component { get; set; }

    [Parameter, EditorRequired]
    public required EventCallback<TComponent> ComponentChanged { get; set; }

    [CascadingParameter]
    public required IPageTemplate Template { get; set; } 

    public async Task UpdateComponent(TComponent component)
    {
        Component = component;
        await ComponentChanged.InvokeAsync(component);
    }
}