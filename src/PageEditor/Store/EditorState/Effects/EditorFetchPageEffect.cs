using System.Collections.Immutable;
using Fluxor;
using StudentPortal.ComponentData.Abstractions;
using StudentPortal.ComponentData.Conversion;
using StudentPortal.Services;

namespace StudentPortal.PageEditor.Storage;

public class EditorFetchPageEffect(IPageStorageService pageStorage, ComponentDataConverter componentDataConverter) : Effect<EditorFetchPageAction>
{
    public override async Task HandleAsync(EditorFetchPageAction action, IDispatcher dispatcher)
    {
        var page = await pageStorage.GetPage(action.Id.ToString());

        if (page != null)
        {
            dispatcher.Dispatch(
                new EditorFetchPageSuccessAction(
                    new PageDataState(
                        IsSaved: true,
                        PageId: page.Id,
                        OwnerId: page.OwnerId,
                        Key: page.Key,
                        Metadata: page.Metadata.ToImmutableDictionary(),
                        Name: page.Name,
                        History: new History<IComponentData>([], [], componentDataConverter.Convert(page.Content))
                    )
                )
            );
        }
        else
        {
            dispatcher.Dispatch(new EditorFetchPageFailAction());
        }
    }
}
