namespace $rootnamespace$
{
    using Tiver.Fowl.TestingBase;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BaseTestForMsTest : IBaseTest
    {
        [TestInitialize]
        public void Setup()
        {
            Flow.Setup(GetType(), TestContext.TestName);
        }

        [TestCleanup]
        public void Teardown()
        {
            Flow.Teardown(TestContext.CurrentTestOutcome);
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
            Flow.SessionTeardown();
        }

        public int? Step
        {
            get;
            set;
        }

        public TestContext TestContext { get; set; }
    }
}