namespace Tiver.Fowl.TestingBase
{
    using System;
    using Core.Context;
    using Core.Enums;
    using Serilog;
    using WebDriverExtended.Browsers;

    public static class Flow
    {
        public static void Setup(Type testType, string testName)
        {
            TestExecutionContext.TestType = testType;
            TestExecutionContext.TestName = testName;

            if (TestExecutionContext.IsWebDriverTest)
            {
                TestExecutionContext.Browser = BrowserFactory.GetBrowser();
                TestExecutionContext.BrowserActions.NavigateToStartUri();
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