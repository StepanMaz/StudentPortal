namespace StudentPortalServer.Entities.Page;

public class Slug
{
    public string Path { get; }

    public Slug(string path)
    {
        Path = NormalizePath(path);
    }

    public Slug Join(string segment)
    {
        return new Slug($"{Path}/{NormalizePath(segment)}");
    }

    public override string ToString()
    {
        return Path;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Slug slug)
            return this.Path.Equals(slug.Path, StringComparison.OrdinalIgnoreCase);
        return false;
    }

    public override int GetHashCode()
    {
        return Path.GetHashCode();
    }

    private static string NormalizePath(string path)
    {
        return path.Trim('/').ToLowerInvariant();
    }

    public static bool operator ==(Slug a, Slug b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Slug a, Slug b)
    {
        return !a.Equals(b);
    }

    public static implicit operator string(Slug slug)
    {
        return slug.Path;
    }

    public static implicit operator Slug(string path)
    {
        return new Slug(path);
    }
}