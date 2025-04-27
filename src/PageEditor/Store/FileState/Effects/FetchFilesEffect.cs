using Fluxor;
using StudentPortal.Services;

namespace StudentPortal.PageEditor.Storage;

public class FetchFilesEffect(IFileService fileService) : Effect<FetchFilesAction>
{
    public override async Task HandleAsync(FetchFilesAction action, IDispatcher dispatcher)
    {
        var res = await fileService.GetFiles();

        if (res != null)
        {
            dispatcher.Dispatch(new FetchFilesSuccessAction(res));
        }
        else
        {
            dispatcher.Dispatch(new FetchFilesFailAction());
        }
    }
}