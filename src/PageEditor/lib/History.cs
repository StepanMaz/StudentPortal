namespace StudentPortal.PageEditor;

public interface IHistory<T>
{
    T Current { get; }
    int UndoCount { get; }
    int RedoCount { get; }
    void Push(T state);
    T Undo();
    T Redo();
    void Clear();
}

public class History<T> : IHistory<T>
{
    private Stack<T> _undo = [];
    private Stack<T> _redo = [];

    private T _current = default!;

    public T Current => _current;

    public int UndoCount => _undo.Count;

    public int RedoCount => _redo.Count;

    public void Push(T state)
    {
        _undo.Push(_current);
        _current = state;
        _redo.Clear();
    }

    public T Undo()
    {
        if (_undo.Count > 0)
        {
            _redo.Push(_current);
            _current = _undo.Pop();
        }

        return _current;
    }

    public T Redo()
    {
        if (_redo.Count > 0)
        {
            _undo.Push(_current);
            _current = _redo.Pop();
        }

        return _current;
    }

    public void Clear()
    {
        _undo.Clear();
        _redo.Clear();
    }
}

public static class IHistoryExtensions
{
    public static bool CanUndo<T>(this IHistory<T> history)
    {
        return history.UndoCount > 0;
    }

    public static bool CanRedo<T>(this IHistory<T> history)
    {
        return history.RedoCount > 0;
    }
}