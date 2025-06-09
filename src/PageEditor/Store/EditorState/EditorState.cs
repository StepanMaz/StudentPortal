using System.Collections.Immutable;
using Fluxor;
using StudentPortal.ComponentData.Abstractions;
using StudentPortal.Services;

namespace StudentPortal.PageEditor.Storage;

public enum EditorDisplayMode
{
    Editor,
    Preview,
    Combined
}

public record PageDataState(bool IsSaved, Guid OwnerId, string Key, Guid PageId, ImmutableDictionary<string, object> Metadata, string Name, IImmutableHistory<IComponentData> History);

[FeatureState]
public record EditorState(bool IsLoading, PageDataState Page, EditorDisplayMode DisplayMode)
{
    public EditorState() : this(true, default!, EditorDisplayMode.Editor) { }
}

public record EditorFetchPageAction(Guid Id);
public record EditorFetchPageSuccessAction(PageDataState Page);
public record EditorFetchPageFailAction;

public record EditorSavePageAction;
public record EditorSavePageSuccessAction(PageData PageData);
public record EditorSavePageFailAction;

public record EditorHistoryUndoAction;
public record EditorHistoryRedoAction;
public record EditorHistoryPushAction(IComponentData State);

public record EditorSetNameAction(string Name);

public record EditorSetDisplayModeAction(EditorDisplayMode DisplayMode);