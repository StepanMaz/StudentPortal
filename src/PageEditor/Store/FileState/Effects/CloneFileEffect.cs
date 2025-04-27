using Fluxor;
using StudentPortal.Services;

namespace StudentPortal.PageEditor.Storage;

public class CloneFileEffect(IPageStorageService pageStorage) : Effect<CloneFileAction>
{
    public override async Task HandleAsync(CloneFileAction action, IDispatcher dispatcher)
    {
        var page = await pageStorage.GetPage(action.File.Id);

        if (page == null)
        {
            dispatcher.Dispatch(new CloneFileFailAction(action.File, CloneFileActionFailReason.FailedToGet));
            return;
        }

        var update = new PageData()
        {
            Id = page.Id,
            Content = page.Content,
            Metadata = page.Metadata,
            Name = "Copy of " + page.Name
        };

        update = await pageStorage.CreatePage(update);

        dispatcher.Dispatch(new CloneFileSuccessAction(new SPFileInfo() { Id = update.Id.ToString(), Name = update.Name }));
    }
}
