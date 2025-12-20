#if TIVER_NUNIT
namespace Tiver.Fowl.TestingBase
{
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;
    using Tiver.Fowl.Core.Context;
    using Tiver.Fowl.Core.Enums;

    /// <summary>
    /// Base test class for NUnit tests.
    /// Requires TIVER_NUNIT to be defined in the consuming project.
    /// Uses fully qualified test name (Namespace.Class.Method) for proper hierarchy in reports.
    /// </summary>
    [TestFixture]
    public class BaseTestForNUnit : IBaseTest
    {
        [SetUp]
        public void Setup()
        {
            Flow.Setup(
                GetType(),
                TestContext.CurrentContext.Test.FullName,
                () => TestContext.CurrentContext.Test.FullName);
        }

        [TearDown]
        public void Teardown()
        {
            var testStatus = TestContext.CurrentContext.Result.Outcome.Status;
            TestResult testResult;
            switch (testStatus)
            {
                case TestStatus.Passed:
                    testResult = TestResult.Passed;
                    break;
                case TestStatus.Failed:
                    testResult = TestResult.Failed;
                    break;
                case TestStatus.Inconclusive:
                case TestStatus.Skipped:
                case TestStatus.Warning:
                default:
                    testResult = TestResult.Unknown;
                    break;
            }

            Flow.Teardown(testResult);
        }
    }

    /// <summary>
    /// NUnit setup fixture for session-level initialization.
    /// </summary>
    [SetUpFixture]
    public class SetupFixtureForNUnit
    {
        [OneTimeSetUp]
        public static void Initialize()
        {
            Tiver.Fowl.Logging.Logger.Configure();
        }

        [OneTimeTearDown]
        public static void Cleanup()
        {
            Flow.SessionTeardown();
        }
    }
}
#endif
