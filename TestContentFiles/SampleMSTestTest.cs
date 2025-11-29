#if TIVER_MSTEST
namespace TestContentFiles
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tiver.Fowl.TestingBase;

    /// <summary>
    /// Sample test to validate BaseTestForMSTest compiles and works correctly.
    /// </summary>
    [TestClass]
    public class SampleMSTestTest : BaseTestForMSTest
    {
        [TestMethod]
        public void SampleTest_ShouldPass()
        {
            // This test validates that:
            // 1. BaseTestForMSTest compiles correctly
            // 2. Setup/Teardown methods work
            // 3. MSTest integration is functional
            Assert.IsTrue(true, "BaseTestForMSTest validation passed");
        }
    }
}
#endif
