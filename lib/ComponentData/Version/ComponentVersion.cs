using System.Diagnostics.CodeAnalysis;

namespace StudentPortal.ComponentData;

public record struct ComponentVersion(int Major, int Minor, int Patch) : IParsable<ComponentVersion>
{
    public static readonly ComponentVersion Empty = new ComponentVersion(0, 0, 0);

    public override string ToString()
    {
        return $"{Major}.{Minor}.{Patch}";
    }

    public static ComponentVersion Parse(string s, IFormatProvider? provider = null)
    {
        const int LENGTH = 3;

        var parts = s.Split('.');

        if (parts.Length != LENGTH)
            throw new FormatException("Could not parse version string. Version string must be in the format 'Major.Minor.Patch'.");

        Span<int> numbers = stackalloc int[LENGTH];

        for (int i = 0; i < LENGTH; i++)
        {
            int parsed;

            if (!int.TryParse(parts[i], out parsed))
            {
                throw new FormatException("Could not parse version string. Version parts should be positive integers.");
            }

            numbers[i] = parsed;
        }

        return new ComponentVersion(numbers[0], numbers[1], numbers[2]);
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ComponentVersion result)
    {
        if (s is null)
        {
            result = Empty;
            return false;
        }

        try
        {
            result = Parse(s, provider);
            return true;
        }
        catch
        {
            result = Empty;
            return false;
        }
    }
}