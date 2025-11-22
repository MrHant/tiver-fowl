using NUnit.Framework;
using Tiver.Fowl.TestingBase;

namespace Tests.NUnit;

[TestFixture]
public class SampleTest : BaseTestForNUnit
{
    [Test]
    public void BasicTest()
    {
        Assert.That(true, Is.True);
    }
}
