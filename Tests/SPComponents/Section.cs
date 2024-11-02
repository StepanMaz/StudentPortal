
using StudentPortalServer.Models.Components;

namespace Tests.SPComponents;

public class SectionComponentDataTests
{
    private ISPComponent AlternativeTestData => new MarkdownComponentData("some random content");
    private ISPComponent TestData => new SectionComponentData([AlternativeTestData]);

    [Fact]
    public void Equality_ShouldReturnTrueOnItself()
    {
        ISPComponent data = TestData;

        Assert.True(data.Equals(data));
    }

    [Fact]
    public void Equality_ShouldReturnTrueOnSameData()
    {
        ISPComponent data1 = TestData;
        ISPComponent data2 = TestData;

        Assert.True(data1.Equals(data2));
    }

    [Fact]
    public void Equality_ShouldReturnFalseOnNull()
    {
        ISPComponent data1 = TestData;
        ISPComponent data2 = null!;

        Assert.False(data1.Equals(data2));
    }

    [Fact]
    public void Equality_ShouldReturnFalseOnAnotherComponent()
    {
        ISPComponent data1 = TestData;
        ISPComponent data2 = AlternativeTestData;

        Assert.False(data1.Equals(data2));
    }
}