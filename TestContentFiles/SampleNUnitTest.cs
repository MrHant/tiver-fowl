#if TIVER_NUNIT
namespace TestContentFiles
{
    using NUnit.Framework;
    using Tiver.Fowl.TestingBase;

    /// <summary>
    /// Sample test to validate BaseTestForNUnit compiles and works correctly.
    /// </summary>
    [TestFixture]
    public class SampleNUnitTest : BaseTestForNUnit
    {
        [Test]
        public void SampleTest_ShouldPass()
        {
            // This test validates that:
            // 1. BaseTestForNUnit compiles correctly
            // 2. Setup/Teardown methods work
            // 3. NUnit integration is functional
            Assert.Pass("BaseTestForNUnit validation passed");
        }
    }
}
#endif
