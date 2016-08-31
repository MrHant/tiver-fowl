namespace Tiver.Fowl.TestingBase
{
    using Core.Context;
    using WebDriverExtended.Browsers;

    public static class Flow
    {
        public static void Setup()
        {
            if (TestExecutionContext.IsWebDriverTest)
            {
                TestExecutionContext.Browser = BrowserFactory.GetBrowser();
                TestExecutionContext.Browser.NavigateToStartUri();
            }
        }

        public static void Teardown()
        {
            if (TestExecutionContext.IsWebDriverTest)
            {
                TestExecutionContext.Browser.Quit();
            }
        }
    }
}