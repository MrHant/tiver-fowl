namespace Tiver.Fowl.WebDriverExtended.Contracts.Browsers
{
    using OpenQA.Selenium;

    public interface IBrowser
    {
        void NavigateToStartUri();

        IWebElement Find(string locator);

        void Quit();
    }
}
