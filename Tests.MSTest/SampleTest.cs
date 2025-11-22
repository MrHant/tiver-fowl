using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.MSTest;

[TestClass]
public class SampleTest
{
    [TestMethod]
    public void BasicTest()
    {
        Assert.IsTrue(true);
    }
}
