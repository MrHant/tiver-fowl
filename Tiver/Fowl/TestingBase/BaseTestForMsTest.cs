namespace Tiver.Fowl.TestingBase
{
    using Core.Context;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Reporting;
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
            Log.ForContext("LogType", "Outcome").Information("Test outcome - '{outcome}'", TestContext.CurrentTestOutcome);
            Flow.Teardown();
            Context.ClearTestContext();
        }

        /// <summary>
        /// Assembly cleanup won't run from other assembly
        /// This method must me triggered from Test assembly
        /// </summary>
        public static void AssemblyTeardown()
        {
            Context.ClearSessionContext();
            Report.Build();
        }

        public TestContext TestContext { get; set; }
    }
}