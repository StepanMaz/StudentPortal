using StudentPortal.ComponentData.Conversion;

namespace StudentPortal.Testing;

public class ComponentsConversionTest
{
    DocumentConverter converter = default!;

    [SetUp]
    public void SetUp()
    {
        converter = new DocumentConverter();
    }

    private record ValueObj<T>(T Value);

    [Test]
    public void ComponentPropertyShouldBeInComponentDict()
    {
        
    }
}