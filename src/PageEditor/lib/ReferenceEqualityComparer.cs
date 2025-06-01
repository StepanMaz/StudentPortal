public class ReferenceEqualityComparer<T> : IEqualityComparer<T>
    where T : class
{
    public bool Equals(T? x, T? y)
    {
        return ReferenceEquals(x, y);
    }

    public int GetHashCode(T obj)
    {
        return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
    }

    public static ReferenceEqualityComparer<T> Instance { get; } = new ReferenceEqualityComparer<T>();
}