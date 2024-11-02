namespace StudentPortalServer.UI.Components.Display;

using Microsoft.AspNetCore.Components;
using StudentPortalServer.Models.Components;

public interface IRenderer
{
    public RenderFragment Render(ISPComponent component);
}
