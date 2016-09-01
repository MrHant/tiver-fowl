namespace Tiver.Fowl.WebDriverExtended.Browsers
{
    using OpenQA.Selenium;

    public class ChromeBrowser : Browser
    {
        internal ChromeBrowser(IWebDriver webDriver)
            : base(webDriver)
        {
        }
    }
}