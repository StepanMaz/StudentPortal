namespace StudentPortal.PageEditor;

public enum EditorDisplayMode
{
    Editor,
    Preview,
    Combined
}

public record EditorState(EditorDisplayMode DisplayMode);