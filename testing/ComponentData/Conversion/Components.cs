using System.Collections.Immutable;
using System.Numerics;
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
        reg.Register<ComplexComponent>();
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

    record Data(int a, int b);
    record ComplexComponent(int b, string c, Data d, IImmutableList<IComponentData> e, IComponentData ComponentData) : IComponentData
    {
        public T Accept<T>(IComponentDataVisitor<T> visitor)
        {
            throw new NotImplementedException();
        }
    }

    [Test]
    public void ShouldSerializeComplexComponent()
    {
        var value = new ComplexComponent(13, "text", new Data(0, 0), [new TestComponent([0, 1, 2]), new TestComponent([3, 4, 5])], new TestComponent([5, 6, 7]));
        var doc = documentConverter.Convert(value);
        var component = componentConverter.Convert(doc) as ComplexComponent;

        Assert.That(JsonSerializer.Serialize(component), Is.EqualTo(JsonSerializer.Serialize(value)));
    }
}