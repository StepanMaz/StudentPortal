using Fluxor;
using StudentPortal.ComponentData.Conversion;
using StudentPortal.Services;

namespace StudentPortal.PageEditor.Storage;

public class EditorSavePageEffect(IState<EditorState> state, IPageStorageService pageStorage, DocumentConverter documentConverter) : Effect<EditorSavePageAction>
{
    public override async Task HandleAsync(EditorSavePageAction action, IDispatcher dispatcher)
    {
        var pageData = state.Value.Page;

        if (state.Value.Page == null)
        {
            dispatcher.Dispatch(new EditorSavePageFailAction());
            return;
        }

        var update = new PageData()
        {
            Id = pageData.PageId,
            Name = pageData.Name,
            Content = documentConverter.Convert(pageData.History.Current),
            Metadata = pageData.Metadata.ToDictionary()
        };

        var res = await pageStorage.UpdatePage(update);

        if (res)
        {
            dispatcher.Dispatch(new EditorSavePageSuccessAction());
        }
        else
        {
            dispatcher.Dispatch(new EditorSavePageFailAction());
        }
    }
}
