namespace Tiver.Fowl.TestingBase
{
    using System;
    using System.IO;
    using Core.Context;
    using Core.Enums;
    using Core.Reporting;
    using Serilog;
    using WebDriverExtended.Browsers;

    public static class Flow
    {
        /// <summary>
        /// Starts a test. Must be called from the test's synchronous setup method — the ambient
        /// test scope is installed here, and an async setup would not propagate it to the test body.
        /// </summary>
        public static void Setup(Type testType, string testName)
        {
            Context.BeginTestScope();
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
            // Generate HTML report for current session only
            try
            {
                var currentSessionId = TestExecutionContext.SessionId;
                var logPath = Path.Combine(Directory.GetCurrentDirectory(), "log.txt");

                if (!string.IsNullOrEmpty(currentSessionId) && File.Exists(logPath))
                {
                    var sessions = LogFileParser.Parse(logPath);
                    if (sessions.TryGetValue(currentSessionId, out var results))
                    {
                        var reportPath = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            $"test-report-{currentSessionId}.html");

                        HtmlReportGenerator.Generate(results, reportPath);
                        Log.Information("Test report generated: {ReportPath}", reportPath);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to generate test report");
            }

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
