using Fluxor;
using StudentPortal.ComponentData.Components;
using StudentPortal.ComponentData.Conversion;
using StudentPortal.Services;

namespace StudentPortal.PageEditor.Storage;

public class AddFileEffect(DocumentConverter documentConverter, IPageStorageService pageStorage) : Effect<AddFileAction>
{
    public override async Task HandleAsync(AddFileAction action, IDispatcher dispatcher)
    {
        var res = await pageStorage.CreatePage(new PageData()
        {
            Name = "New Page",
            Metadata = new Dictionary<string, object>() { ["Template"] = "empty" },
            Content = documentConverter.Convert(new RootComponent(new SectionComponent([], SectionDisplayMode.Pages)))
        });

        if (res != null)
        {
            dispatcher.Dispatch(new AddFileSuccessAction(new SPFileInfo() { Id = res.Id.ToString(), Name = res.Name }));
        }
        else
        {
            dispatcher.Dispatch(new AddFileFailAction());
        }
    }
}
