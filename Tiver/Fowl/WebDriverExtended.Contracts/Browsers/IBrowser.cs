namespace Tiver.Fowl.WebDriverExtended.Contracts.Browsers
{
    using OpenQA.Selenium;

    public interface IBrowser
    {
        void NavigateToStartUri();

        IWebElement Find(string locator);

        object ExecuteScript(string script, params object[] arguments);

        void TakeScreenshot();

        void Quit();
    }
}
