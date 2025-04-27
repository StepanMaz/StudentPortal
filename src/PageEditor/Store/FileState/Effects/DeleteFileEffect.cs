using Fluxor;
using StudentPortal.Services;

namespace StudentPortal.PageEditor.Storage;

public class DeleteFileEffect(IPageStorageService pageStorage) : Effect<DeleteFileAction>
{
    public override async Task HandleAsync(DeleteFileAction action, IDispatcher dispatcher)
    {
        var success = await pageStorage.DeletePage(action.File.Id);

        if (success)
        {
            dispatcher.Dispatch(new DeleteFileSuccessAction(action.File));
        }
        else
        {
            dispatcher.Dispatch(new DeleteFileFailAction(action.File));
        }
    }
}
