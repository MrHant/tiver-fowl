namespace Tiver.Fowl.TestingBase
{
    using System;
    using Core.Context;
    using Core.Enums;
    using Serilog;
    using WebDriverExtended.Browsers;

    public static class Flow
    {
        public static void Setup(Type testType, string testName, Func<string> testKey)
        {
            Context.SetTestKey(testKey);
            TestExecutionContext.TestName = testName;
            TestExecutionContext.TestType = testType;

            if (TestExecutionContext.IsWebDriverTest)
            {
                TestExecutionContext.Browser = BrowserFactory.GetBrowser();
            }
        }

        public static void Teardown(TestResult testResult)
        {
            TestExecutionContext.TestResult = testResult;
            Teardown();
        }

        public static void SessionTeardown()
        {
            Context.ClearSessionContext();
        }

        private static void Teardown()
        {
            var logResult = Log.ForContext("LogType", "TestResult");
            logResult.Information("Test result - '{TestResult}'", TestExecutionContext.TestResult);

            if (TestExecutionContext.IsWebDriverTest)
            {
                if (TestExecutionContext.TestResult == TestResult.Failed)
                {
                    TestExecutionContext.BrowserActions.TakeScreenshot();
                }

                TestExecutionContext.BrowserActions.Quit();
            }

            Context.ClearTestContext();
        }
    }
}