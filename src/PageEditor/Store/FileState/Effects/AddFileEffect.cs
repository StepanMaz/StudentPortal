using Fluxor;
using StudentPortal.ComponentData.Conversion;
using StudentPortal.Services;

namespace StudentPortal.PageEditor.Storage;

public class AddFileEffect(IState<FilesState> filesState, DocumentConverter documentConverter, IPageStorageService pageStorage) : Effect<AddFileAction>
{
    public override async Task HandleAsync(AddFileAction action, IDispatcher dispatcher)
    {
        var res = await pageStorage.CreatePage(new PageData()
        {
            Name = GetFileName(),
            Metadata = new Dictionary<string, object>() { ["Template"] = action.Template.DisplayName.ToLower() },
            Content = documentConverter.Convert(action.Template.PageTemplate)
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

    private string GetFileName()
    {
        var name = "New Page";

        if (!filesState.Value.Files.Any(x => x.Name == name)) return name;

        int index = 1;
        while (filesState.Value.Files.Any(x => x.Name == $"{name} ({index})"))
        {
            index++;
        }

        return $"{name} ({index})";
    }
}
