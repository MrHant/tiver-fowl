namespace Tiver.TestingBase
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tiver.Core.Context;
    using Tiver.TestingBase.Contracts;

    public class BaseTestForMsTest : BaseTest, IBaseTest
    {
        [TestInitialize]
        public void SetUp()
        {
            Flow.SetUp();
        }

        [TestCleanup]
        public void TearDown()
        {
            Flow.TearDown();
            Context.ClearTestContext();
        }

        [AssemblyCleanup]
        public void AssemblyTearDown()
        {
            Context.ClearSessionContext();
        }
    }
}