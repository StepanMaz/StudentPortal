using System.Text.Json;
using StudentPortal.ComponentData.Abstractions;
using StudentPortal.ComponentData.Conversion;

namespace StudentPortal.Testing;

public class ComponentsConversionTest
{
    DocumentConverter documentConverter = default!;
    ComponentDataConverter componentConverter = default!;

    [SetUp]
    public void SetUp()
    {
        documentConverter = new DocumentConverter();
        var reg = new TypeRegistry();
        reg.Register<TestComponent>();
        reg.Register<ValueObj<TestComponent>>();
        componentConverter = new ComponentDataConverter(reg);

    }

    private record ValueObj<T>(T Value) : IComponentData
    {
        public T1 Accept<T1>(IComponentDataVisitor<T1> visitor)
        {
            throw new NotImplementedException();
        }
    }


    public record TestComponent(int[] Ints) : IComponentData
    {
        public T Accept<T>(IComponentDataVisitor<T> visitor)
        {
            throw new NotImplementedException();
        }
    }


    [Test]
    public void ComponentPropertyShouldBeInComponentDict()
    {
        var value = new TestComponent([1]);
        var obj = new ValueObj<TestComponent>(value);

        var document = documentConverter.Convert(obj);
        
        Assert.That(document.Components, Is.Not.Null);
        Assert.That(document.Components.ContainsKey(nameof(ValueObj<object>.Value)));

        var component = componentConverter.Convert(document) as ValueObj<TestComponent>;

        Assert.That(JsonSerializer.Serialize(component), Is.EqualTo(JsonSerializer.Serialize(obj)));
    }
}