using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tiver.Fowl.TestingBase;

namespace Tests.MSTest;

[TestClass]
public class SampleTest : BaseTestForMSTest
{
    [TestMethod]
    public void BasicTest()
    {
        Assert.IsInstanceOfType<BaseTestForMSTest>(this);
    }
}
