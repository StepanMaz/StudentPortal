using System.Collections.Immutable;
using Fluxor;
using StudentPortal.PageEditor.Templates;
using StudentPortal.Services;

namespace StudentPortal.PageEditor.Storage;

[FeatureState]
public record FilesState(ImmutableList<SPFileInfo> Files)
{
    public FilesState() : this([]) { }

    public void FetchFiles() {

    }
}

public record FetchFilesAction;
public record FetchFilesSuccessAction(IEnumerable<SPFileInfo> Files);
public record FetchFilesFailAction;

public record AddFileAction(IPageTemplate Template);
public record AddFileSuccessAction(SPFileInfo File);
public record AddFileFailAction;

public record RenameFileAction(SPFileInfo File, string newName);
public record RenameFileSuccessAction(SPFileInfo From, SPFileInfo To);
public record RenameFileFailAction(SPFileInfo From);

public record DeleteFileAction(SPFileInfo File);
public record DeleteFileSuccessAction(SPFileInfo File);
public record DeleteFileFailAction(SPFileInfo File);

public record CloneFileAction(SPFileInfo File);
public record CloneFileSuccessAction(SPFileInfo File);
public enum CloneFileActionFailReason {
    FailedToGet,
    FailedToClone
}
public record CloneFileFailAction(SPFileInfo File, CloneFileActionFailReason Reason);