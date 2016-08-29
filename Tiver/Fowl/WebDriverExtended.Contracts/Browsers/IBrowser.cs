namespace Tiver.Fowl.WebDriverExtended.Contracts.Browsers
{
    using OpenQA.Selenium;

    public interface IBrowser
    {
        IWebElement Find(string locator);

        void Quit();
    }
}
