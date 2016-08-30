namespace Tiver.Fowl.TestingBase
{
    using Core.Context;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class BaseTestForMSTest : BaseTest
    {
        [TestInitialize]
        public override void Setup()
        {
            Flow.Setup();
        }

        [TestCleanup]
        public override void Teardown()
        {
            Flow.Teardown();
            Context.ClearTestContext();
        }

        [AssemblyCleanup]
        public void AssemblyTeardown()
        {
            Context.ClearSessionContext();
        }
    }
}