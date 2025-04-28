using System.Collections.Immutable;

namespace StudentPortal.PageEditor;

public interface IImmutableHistory<T>
{
    T Current { get; }

    bool CanUndo { get; }
    bool CanRedo { get; }

    IImmutableHistory<T> Push(T state);
    IImmutableHistory<T> Undo();
    IImmutableHistory<T> Redo();
    IImmutableHistory<T> Clear();
}

public class History<T>(ImmutableStack<T> undo, ImmutableStack<T> redo, T current) : IImmutableHistory<T>
{
    private History() : this([], [], default!) { }

    public T Current => current;

    public bool CanUndo => !undo.IsEmpty;
    public bool CanRedo => !redo.IsEmpty;

    public IImmutableHistory<T> Push(T state)
    {
        return new History<T>(undo.Push(current), [], state);
    }

    public IImmutableHistory<T> Undo()
    {
        if (CanUndo)
        {
            var previous = undo.Peek();
            return new History<T>(undo.Pop(), redo.Push(current), previous);
        }

        return this;
    }

    public IImmutableHistory<T> Redo()
    {
        if (CanRedo)
        {
            var next = redo.Peek();
            return new History<T>(undo.Push(current), redo.Pop(), next);
        }

        return this;
    }

    public IImmutableHistory<T> Clear()
    {
        return Empty();
    }

    public static IImmutableHistory<T> Empty() => new History<T>();

    public override string ToString()
    {
        return $"[{string.Join(", ", undo)}] - {Current} - [{string.Join(",", redo)}]";
    }
}