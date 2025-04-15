namespace StudentPortal.PageEditor;

public static class LabeledEnumerableExtensions
{
    public static IEnumerable<(string label, T value)> Labeled<T>(this IEnumerable<T> values)
    {
        int count = 0;
        foreach (var item in values)
        {
            yield return (ToLabel(count), item);
            count++;
        }
    }

    private static string ToLabel(int number)
    {
        string res = "";

        if (number == 0) return "A";

        while (number >= 0)
        {
            res = (char)('A' + (number % 26)) + res;

            number = number / 26 - 1;

            if (number < 0) break;
        }

        return res;
    }
}