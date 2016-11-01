namespace Tiver.Fowl.Core.Enums
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static class TestResultExtensions
    {
        public static TestResult ConvertToTestResult(this UnitTestOutcome testOutcome)
        {
            switch (testOutcome)
            {
                case UnitTestOutcome.Passed:
                    return TestResult.Passed;
                case UnitTestOutcome.Failed:
                    return TestResult.Failed;
                case UnitTestOutcome.Inconclusive:
                case UnitTestOutcome.InProgress:
                case UnitTestOutcome.Error:
                case UnitTestOutcome.Timeout:
                case UnitTestOutcome.Aborted:
                case UnitTestOutcome.Unknown:
                default:
                    return TestResult.Unknown;
            }
        }
    }
}
