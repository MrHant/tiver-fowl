namespace Tiver.TestingBase
{
    using Tiver.WebDriverExtended.Browsers;

    public class Flow
    {
        public static void SetUp(TestExecutionContext context)
        {
            if (context.IsWebDriverTest)
            {
                context.Browser = BrowserFactory.GetBrowser();
            }
        }

        public static void TearDown(TestExecutionContext context)
        {
            if (context.IsWebDriverTest)
            {
                context.Browser.Quit();
            }
        }
    }
}