namespace Tiver.Fowl.WebDriverExtended.Browsers
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