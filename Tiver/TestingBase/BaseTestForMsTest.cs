namespace Tiver.TestingBase
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tiver.Core.Context;
    using Tiver.TestingBase.Contracts;

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