using System.Collections.Immutable;
using Fluxor;

namespace StudentPortal.PageEditor.Storage;

public static class FileStateReducers
{
    [ReducerMethod]
    public static FilesState ReduceFetchFilesAction(FilesState _, FetchFilesSuccessAction action)
    {
        return new (action.Files.ToImmutableList());
    }

    [ReducerMethod]
    public static FilesState ReduceAddFile(FilesState state, AddFileSuccessAction action)
    {
        return new (state.Files.Add(action.File));
    }

    [ReducerMethod]
    public static FilesState ReduceRenameFile(FilesState state, RenameFileSuccessAction action)
    {
        return new(state.Files.Replace(action.From, action.To));
    }

    [ReducerMethod]
    public static FilesState ReduceDeleteFile(FilesState state, DeleteFileSuccessAction action)
    {
        return new(state.Files.Remove(action.File));
    }

    [ReducerMethod]
    public static FilesState ReduceCloneFile(FilesState state, CloneFileSuccessAction action)
    {
        return new(state.Files.Add(action.File));
    }
}