namespace Tiver.Fowl.WebDriverExtended.Browsers
{
    using System.Configuration;
    using System.Linq;
    using Contracts.Browsers;
    using Core.Configuration;
    using OpenQA.Selenium;
    using Serilog;

    public abstract class Browser : IBrowser, IWebElementActions, IBrowserActions
    {
        public IBrowserActions BrowserActions => this;

        public IWebElementActions WebElementActions => this;

        #region IBrowserActionsImplementation

        public void NavigateToStartUri()
        {
            IApplicationConfiguration config =
                (ApplicationConfigurationSection)
                    ConfigurationManager.GetSection("applicationConfigurationGroup/applicationConfiguration");
            webDriver.Navigate().GoToUrl(config.StartUrl);
        }

        public void SwitchToFrame(string locator)
        {
            webDriver.SwitchTo().Frame(Find(locator));
        }

        public void SwitchToMainFrame()
        {
            webDriver.SwitchTo().DefaultContent();
        }

        public void TakeScreenshot()
        {
            var ss = ((ITakesScreenshot) webDriver).GetScreenshot();
            var base64 = ss.AsBase64EncodedString;
            Log.ForContext("LogType", "Screenshot")
                .ForContext("Base64", base64)
                .Information($"{{Name}} :: {{Action}}", "Browser", "Screenshot taken");
        }

        public void Quit()
        {
            this.webDriver?.Quit();
        }

        public object ExecuteScript(string script, params object[] arguments)
        {
            return ((IJavaScriptExecutor) webDriver).ExecuteScript(script, arguments);
        }

        #endregion

        #region IWebElementActions implementation

        public IWebElement Find(string locator)
        {
            var elements = this.webDriver.FindElements(By.XPath(locator));
            if (elements.Count == 1)
            {
                return elements.Single();
            }
            throw new NoSuchElementException();
        }

        #endregion

        internal Browser(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
        }

        private readonly IWebDriver webDriver;
    }
}