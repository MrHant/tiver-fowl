namespace Tiver.Fowl.TestingBase
{
    using Core.Context;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class BaseTestForMsTest : BaseTest
    {
        [TestInitialize]
        public override void SetUp()
        {
            Flow.SetUp();
        }

        [TestCleanup]
        public override void TearDown()
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