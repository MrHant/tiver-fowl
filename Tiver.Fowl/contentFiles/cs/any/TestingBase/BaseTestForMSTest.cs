#if TIVER_MSTEST
namespace Tiver.Fowl.TestingBase
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tiver.Fowl.Core.Context;
    using Tiver.Fowl.Core.Enums;

    /// <summary>
    /// Base test class for MSTest tests.
    /// Requires TIVER_MSTEST to be defined in the consuming project.
    /// Uses fully qualified test name (Namespace.Class.Method) for proper hierarchy in reports.
    /// </summary>
    [TestClass]
    public class BaseTestForMSTest : IBaseTest
    {
        public TestContext TestContext { get; set; }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            Tiver.Fowl.Logging.Logger.Configure();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Flow.SessionTeardown();
        }

        [TestInitialize]
        public void Setup()
        {
            // Construct full name similar to NUnit's FullName for proper namespace hierarchy
            var testType = GetType();
            var fullName = $"{testType.Namespace}.{testType.Name}.{TestContext.TestName}";

            Flow.Setup(testType, fullName);
        }

        [TestCleanup]
        public void Teardown()
        {
            var outcome = TestContext.CurrentTestOutcome;
            Core.Enums.TestResult testResult;
            switch (outcome)
            {
                case UnitTestOutcome.Passed:
                    testResult = Core.Enums.TestResult.Passed;
                    break;
                case UnitTestOutcome.Failed:
                    testResult = Core.Enums.TestResult.Failed;
                    break;
                case UnitTestOutcome.Inconclusive:
                case UnitTestOutcome.Error:
                case UnitTestOutcome.Timeout:
                case UnitTestOutcome.NotRunnable:
                case UnitTestOutcome.Unknown:
                default:
                    testResult = Core.Enums.TestResult.Unknown;
                    break;
            }

            Flow.Teardown(testResult);
        }
    }
}
#endif
