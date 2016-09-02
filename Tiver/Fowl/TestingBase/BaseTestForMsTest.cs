namespace Tiver.Fowl.TestingBase
{
    using Core.Context;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Serilog;

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
            Log.Information("Test outcome - '{outcome}'", TestContext.CurrentTestOutcome);
        }

        [AssemblyCleanup]
        public void AssemblyTeardown()
        {
            Context.ClearSessionContext();
        }

        public TestContext TestContext { get; set; }
    }
}