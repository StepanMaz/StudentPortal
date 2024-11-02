using StudentPortalServer.Models.Components;

namespace Tests.SPComponents;

public class MarkdownComponentDataTests
{
    [Fact]
    public void Equality_ShouldReturnTrueOnItself()
    {
        ISPComponent data = new MarkdownComponentData("some random content");

        Assert.True(data.Equals(data));
    }

    [Fact]
    public void Equality_ShouldReturnTrueOnSameData()
    {
        ISPComponent data1 = new MarkdownComponentData("some random content");
        ISPComponent data2 = new MarkdownComponentData("some random content");

        Assert.True(data1.Equals(data2));
    }

    [Fact]
    public void Equality_ShouldReturnFalseOnNull()
    {
        ISPComponent data1 = new MarkdownComponentData("some random content");
        ISPComponent data2 = null!;

        Assert.False(data1.Equals(data2));
    }

    [Fact]
    public void Equality_ShouldReturnFalseOnAnotherComponent()
    {
        ISPComponent data1 = new MarkdownComponentData("some random content");
        ISPComponent data2 = new SectionComponentData([]);

        Assert.False(data1.Equals(data2));
    }
}