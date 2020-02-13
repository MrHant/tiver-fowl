namespace Tests
{
    using Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tiver.Fowl.TestingBase;
using TestResult = Tiver.Fowl.Core.Enums.TestResult;

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

    [AssemblyInitialize]
    public static void Initialize(TestContext context)
    {
        Logger.Configure();
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