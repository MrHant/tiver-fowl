namespace Tiver.TestingBase
{
    using Tiver.Core.Context;
    using Tiver.WebDriverExtended.Browsers;

    public class Flow
    {
        public static void SetUp()
        {
            if (TestExecutionContext.IsWebDriverTest)
            {
                TestExecutionContext.Browser = BrowserFactory.GetBrowser();
            }
        }

        public static void TearDown()
        {
            if (TestExecutionContext.IsWebDriverTest)
            {
                TestExecutionContext.Browser.Quit();
            }
        }
    }
}