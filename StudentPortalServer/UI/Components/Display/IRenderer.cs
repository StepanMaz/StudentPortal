namespace StudentPortalServer.UI.Components.Display;

using Microsoft.AspNetCore.Components;
using StudentPortalServer.Entities.Page;

public interface IRenderer
{
    public RenderFragment Render(ISPComponent component);
}
