namespace Tiver.Fowl.WebDriverExtended.Contracts.Browsers
{
    using OpenQA.Selenium;

    public interface IBrowser
    {
        IBrowserActions BrowserActions
        {
            get;
        }

        IWebElementActions WebElementActions
        {
            get;
        }
    }
}
