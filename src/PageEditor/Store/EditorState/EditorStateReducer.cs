using Fluxor;

namespace StudentPortal.PageEditor.Storage;

public static class EditorStateReducer
{
    [ReducerMethod]
    public static EditorState ReduceEditorHistoryUndo(EditorState state, EditorHistoryUndoAction _)
    {
        return state with { Page = state.Page with { IsSaved = false, History = state.Page.History.Undo() } };
    }

    [ReducerMethod]
    public static EditorState ReduceEditorHistoryRedo(EditorState state, EditorHistoryRedoAction _)
    {
        return state with { Page = state.Page with { IsSaved = false, History = state.Page.History.Redo() } };
    }

    [ReducerMethod]
    public static EditorState ReduceEditorHistoryPush(EditorState state, EditorHistoryPushAction action)
    {
        return state with { Page = state.Page with { IsSaved = false, History = state.Page.History.Push(action.State) } };
    }

    [ReducerMethod]
    public static EditorState ReduceEditorSetName(EditorState state, EditorSetNameAction action)
    {
        return state with { Page = state.Page with { IsSaved = false, Name = action.Name } };
    }

    [ReducerMethod]
    public static EditorState ReduceEditorSetDisplayMode(EditorState state, EditorSetDisplayModeAction action)
    {
        return state with { DisplayMode = action.DisplayMode };
    }

    [ReducerMethod]
    public static EditorState ReduceEditorFetchPageSuccess(EditorState state, EditorFetchPageSuccessAction action)
    {
        return state with
        {
            IsLoading = false,
            Page = action.Page
        };
    }

    [ReducerMethod]
    public static EditorState ReduceEditorSavePageSuccess(EditorState state, EditorSavePageSuccessAction _)
    {
        return state with
        {
            Page = state.Page with { IsSaved = true }
        };
    }
}