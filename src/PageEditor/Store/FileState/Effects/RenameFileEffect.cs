using Fluxor;
using StudentPortal.Services;

namespace StudentPortal.PageEditor.Storage;

public class RenameFileEffect(IPageStorageService pageStorage) : Effect<RenameFileAction>
{
    public override async Task HandleAsync(RenameFileAction action, IDispatcher dispatcher)
    {
        var update = new PageData() { Id = Guid.Parse(action.File.Id), Name = action.newName };
        var res = await pageStorage.UpdatePage(update);

        var renamedFile = new SPFileInfo() { Name = action.newName, Id = action.File.Id };

        dispatcher.Dispatch(new RenameFileSuccessAction(action.File, renamedFile));
    }
}