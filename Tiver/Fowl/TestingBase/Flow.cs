namespace Tiver.Fowl.TestingBase
{
    using Core.Context;
    using WebDriverExtended.Browsers;

    public class Flow
    {
        public static void Setup()
        {
            if (TestExecutionContext.IsWebDriverTest)
            {
                TestExecutionContext.Browser = BrowserFactory.GetBrowser();
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