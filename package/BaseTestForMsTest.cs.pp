namespace $rootnamespace$
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tiver.Fowl.Core.Enums;
    using Tiver.Fowl.TestingBase;

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
            var testOutcome = TestContext.CurrentTestOutcome;
            TestResult testResult;
            switch (testOutcome)
            {
                case UnitTestOutcome.Passed:
                    testResult = TestResult.Passed;
                    break;
                case UnitTestOutcome.Failed:
                    testResult = TestResult.Failed;
                    break;
                case UnitTestOutcome.Inconclusive:
                case UnitTestOutcome.InProgress:
                case UnitTestOutcome.Error:
                case UnitTestOutcome.Timeout:
                case UnitTestOutcome.Aborted:
                case UnitTestOutcome.Unknown:
                default:
                    testResult = TestResult.Unknown;
                    break;
            }

            Flow.Teardown(testResult);
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