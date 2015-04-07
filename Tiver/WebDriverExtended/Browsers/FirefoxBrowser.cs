namespace Tiver.WebDriverExtended.Browsers
{
    using OpenQA.Selenium;

    public class FirefoxBrowser : Browser
    {
        internal FirefoxBrowser(IWebDriver webDriver)
            : base(webDriver)
        {
        }
    }
}