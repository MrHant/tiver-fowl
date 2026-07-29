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
}

/// <summary>
/// NUnit setup fixture for session-level initialization: configures logging before the first test
/// and generates the HTML report after the last one.
///
/// Declared outside any namespace on purpose. NUnit scopes a SetUpFixture to its own namespace and
/// that namespace's children, so a fixture inside Tiver.Fowl.TestingBase would never apply to a
/// consumer's test namespace. Only a fixture outside any namespace covers the whole assembly.
/// </summary>
[NUnit.Framework.SetUpFixture]
public class TiverFowlSessionFixture
{
    [NUnit.Framework.OneTimeSetUp]
    public static void Initialize()
    {
        Tiver.Fowl.Logging.Logger.Configure();
    }

    [NUnit.Framework.OneTimeTearDown]
    public static void Cleanup()
    {
        Tiver.Fowl.TestingBase.Flow.SessionTeardown();
    }
}
#endif
