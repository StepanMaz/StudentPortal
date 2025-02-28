using System.Text.Json;
using StudentPortal.ComponentData.Conversion;

namespace StudentPortal.Testing;

public class ComplexObjectsTest
{
    DocumentConverter converter = default!;

    [SetUp]
    public void SetUp()
    {
        converter = new DocumentConverter();
    }
    private record ValueObj<T>(T Value);

    [Test]
    public void ShouldSerializeStringLists()
    {
        List<string> value = ["test"];
        var obj = new ValueObj<List<string>>(value);

        var converted = converter.Convert(obj);

        var key = nameof(ValueObj<object>.Value);

        Assert.That(converted.Properties[key], Is.EqualTo(value));
    }

    [Test]
    public void ShouldSerializeIntLists()
    {
        List<int> value = [42];
        var obj = new ValueObj<List<int>>(value);

        var converted = converter.Convert(obj);

        var key = nameof(ValueObj<object>.Value);

        Assert.That(converted.Properties[key], Is.EqualTo(value));
    }

    [Test]
    public void ShouldSerializeObjectLists()
    {
        List<ValueObj<int>> value = [new ValueObj<int>(42)];
        var obj = new ValueObj<List<ValueObj<int>>>(value);

        var converted = converter.Convert(obj);

        var key = nameof(ValueObj<object>.Value);

        Assert.That(converted.Properties[key], Is.EqualTo(new object[] {"""{"Value":42}"""}));
    }

    [Test]
    public void ShouldSerializeArrays()
    {
        string[] value = ["test"];

        var obj = new ValueObj<string[]>(value);

        var converted = converter.Convert(obj);

        var key = nameof(ValueObj<object>.Value);

        Assert.That(converted.Properties[key], Is.EqualTo(value));
    }
}