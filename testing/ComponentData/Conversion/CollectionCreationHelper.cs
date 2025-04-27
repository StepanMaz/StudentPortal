using System.Collections.Immutable;
using StudentPortal.ComponentData.Conversion;

namespace StudentPortal.Testing;

public class CollectionCreationHelperTests
{
    [Test]
    public void ShouldSerializeArrays()
    {
        int[] array = [1];

        var res = CollectionCreationHelper.CreateCollection(typeof(int[]), array.AsEnumerable().Cast<object>());

        Assert.That(res, Is.EqualTo(array));
    }

    [Test]
    public void ShouldSerializeLists()
    {
        List<int> array = [1];

        var res = CollectionCreationHelper.CreateCollection(typeof(List<int>), array.AsEnumerable().Cast<object>());

        Assert.That(res, Is.EqualTo(array));
    }

    [Test]
    public void ShouldSerializeImmutableList()
    {
        ImmutableList<int> array = [1];

        var res = CollectionCreationHelper.CreateCollection(typeof(ImmutableList<int>), array.AsEnumerable().Cast<object>());

        Assert.That(res, Is.EqualTo(array));
    }
}