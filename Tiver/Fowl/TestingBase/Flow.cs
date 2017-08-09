namespace Tiver.Fowl.TestingBase
{
    using System;
    using Core.Context;
    using Core.Enums;
    using Logging;
    using Reporting;
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
            Report.Build();
        }

        private static void Teardown()
        {
            using (LogProvider.OpenMappedContext("LogType", "TestResult"))
            {
                Logger.InfoFormat("Test result - '{TestResult}'", TestExecutionContext.TestResult);
            }

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

        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
    }
}