namespace $rootnamespace$
{
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;
    using Tiver.Fowl.Core.Enums;
    using Tiver.Fowl.TestingBase;

    [TestFixture]
    public class BaseTestForNUnit : IBaseTest
    {
        [SetUp]
        public void Setup()
        {
            Flow.Setup(GetType(), TestContext.CurrentContext.Test.Name);
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

        public int? Step
        {
            get;
            set;
        }
    }

    [SetUpFixture]
    public class SetupFixtureForNUnit
    {
        [OneTimeTearDown]
        public static void Cleanup()
        {
            Flow.SessionTeardown();
        }
    }
}
