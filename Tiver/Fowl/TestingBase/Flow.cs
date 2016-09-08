namespace Tiver.Fowl.TestingBase
{
    using Core.Context;
    using Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WebDriverExtended.Browsers;

    public static class Flow
    {
        public static void Setup()
        {
            Logger.Configure();

            if (TestExecutionContext.IsWebDriverTest)
            {
                TestExecutionContext.Browser = BrowserFactory.GetBrowser();
                TestExecutionContext.Browser.BrowserActions.NavigateToStartUri();
            }
        }

        public static void Teardown()
        {
            if (TestExecutionContext.IsWebDriverTest)
            {
                if (TestExecutionContext.TestContext.CurrentTestOutcome == UnitTestOutcome.Failed)
                {
                    TestExecutionContext.Browser.BrowserActions.TakeScreenshot();
                }

                TestExecutionContext.Browser.BrowserActions.Quit();
            }
        }
    }
}