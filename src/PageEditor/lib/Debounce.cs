namespace StudentPortal.PageEditor;

public class Debounce<T>(int ms, Action<T> action)
{
    public Debounce(TimeSpan time, Action<T> action) : this((int)time.TotalMilliseconds, action) { }
    private System.Timers.Timer _debounceTimer = null!;

    private T _value = default!;

    public void Push(T value)
    {
        if (EqualityComparer<T>.Default.Equals(_value, value)) return;

        _value = value;

        _debounceTimer?.Dispose();
        _debounceTimer = new(ms);
        _debounceTimer.Elapsed += DebounceElapsed;
        _debounceTimer.Enabled = true;
        _debounceTimer.Start();
    }

    private void DebounceElapsed(object? sender, EventArgs eventArgs)
    {
        _debounceTimer?.Dispose();
        action?.Invoke(_value);
    }
}