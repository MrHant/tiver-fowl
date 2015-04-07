namespace Tiver.TestingBase
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class BaseTestForMsTest : BaseTest, IBaseTest
    {
        [TestInitialize]
        public void SetUp()
        {
            Flow.SetUp(this.Context);
        }

        [TestCleanup]
        public void TearDown()
        {
            Flow.TearDown(this.Context);
        }
    }
}