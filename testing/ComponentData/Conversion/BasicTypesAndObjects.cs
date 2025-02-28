using System.Text.Json;
using StudentPortal.ComponentData.Conversion;

namespace StudentPortal.Testing;

public class ConversionBasicTypesAndObjectsTest
{
    DocumentConverter converter = default!;

    [SetUp]
    public void SetUp()
    {
        converter = new DocumentConverter();
    }

    private record ValueObj<T>(T Value);

    [Test]
    public void ShouldConvertBoolToPropertyKeyValue()
    {
        var obj = new ValueObj<bool>(true);

        var res = converter.Convert(obj);

        Assert.That(res.Properties[nameof(ValueObj<bool>.Value)].Equals(true));
    }

    [Test]
    public void ShouldConvertIntToPropertyKeyValue()
    {
        var value = 42;
        var obj = new ValueObj<int>(value);

        var res = converter.Convert(obj);

        Assert.That(res.Properties[nameof(ValueObj<float>.Value)].Equals(value));
    }

    [Test]
    public void ShouldConvertFloatToPropertyKeyValue()
    {
        var value = 42.422f;
        var obj = new ValueObj<float>(value);

        var res = converter.Convert(obj);

        Assert.That(res.Properties[nameof(ValueObj<float>.Value)].Equals(value));
    }

    [Test]
    public void ShouldConvertStringToPropertyKeyValue()
    {
        var value = "test";
        var obj = new ValueObj<string>(value);

        var res = converter.Convert(obj);

        Assert.That(res.Properties[nameof(ValueObj<string>.Value)].Equals(value));
    }

    [Test]
    public void ShouldConvertObjectToPropertyKeyValue()
    {
        var value = new ValueObj<string>("test");
        var obj = new ValueObj<ValueObj<string>>(value);

        var res = converter.Convert(obj);

        Assert.That(res.Properties[nameof(ValueObj<string>.Value)].Equals(JsonSerializer.Serialize(value)));
    }
}