namespace Tiver.Fowl.TestingBase
{
    using System;
    using Core.Context;
    using Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Reporting;
    using Serilog;
    using WebDriverExtended.Browsers;

    public static class Flow
    {
        public static void Setup(Type testType, string testName)
        {
            Logger.Configure();

            TestExecutionContext.TestType = testType;
            TestExecutionContext.TestName = testName;

            if (TestExecutionContext.IsWebDriverTest)
            {
                TestExecutionContext.Browser = BrowserFactory.GetBrowser();
                TestExecutionContext.Browser.BrowserActions.NavigateToStartUri();
            }
        }

        public static void Teardown(UnitTestOutcome testOutcome)
        {
            TestExecutionContext.TestOutcome = testOutcome;

            Log.ForContext("LogType", "Outcome").Information("Test outcome - '{Outcome}'", TestExecutionContext.TestOutcome);

            if (TestExecutionContext.IsWebDriverTest)
            {
                if (TestExecutionContext.TestOutcome == UnitTestOutcome.Failed)
                {
                    TestExecutionContext.Browser.BrowserActions.TakeScreenshot();
                }

                TestExecutionContext.Browser.BrowserActions.Quit();
            }

            Context.ClearTestContext();
        }

        public static void SessionTeardown()
        {
            Context.ClearSessionContext();
            Report.Build();
        }
    }
}