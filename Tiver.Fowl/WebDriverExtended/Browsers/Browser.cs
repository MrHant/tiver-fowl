namespace Tiver.Fowl.WebDriverExtended.Browsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OpenQA.Selenium;
    using Serilog;

    public abstract class Browser : IBrowser, IWebElementActions, IBrowserActions, IDisposable
    {
        public IBrowserActions BrowserActions => this;

        public IWebElementActions WebElementActions => this;

        #region IBrowserActionsImplementation

        public void NavigateToUrl(Uri url)
        {
            webDriver.Navigate().GoToUrl(url);
        }

        public void Refresh()
        {
            webDriver.Navigate().Refresh();
        }

        public void Back()
        {
            webDriver.Navigate().Back();
        }

        public void Forward()
        {
            webDriver.Navigate().Forward();
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
            try
            {
                var ss = ((ITakesScreenshot) webDriver).GetScreenshot();
                var base64 = ss.AsBase64EncodedString;
                Log.ForContext("LogType", "Screenshot")
                    .ForContext("Base64", base64)
                    .Information($"{{Name}} :: {{Action}}", "Browser", "Screenshot taken");
            }
            catch (Exception e)
            {
                Log.Error(e, "Can't take screenshot");
            }
        }

        public void CloseWindow()
        {
            webDriver.Close();
        }

        public void Quit()
        {
            this.webDriver?.Quit();
        }

        public void Dispose()
        {
            Quit();
            GC.SuppressFinalize(this);
        }

        public object? ExecuteScript(string script, params object[] arguments)
        {
            return ((IJavaScriptExecutor) webDriver).ExecuteScript(script, arguments);
        }

        #endregion

        #region IWebElementActions implementation

        public IWebElement Find(string locator)
        {
            Log.Debug("Find element by locator: {Locator}", locator);
            var elements = this.webDriver.FindElements(By.XPath(locator));
            if (elements.Count == 1)
            {
                return elements.Single();
            }

            throw new NoSuchElementException("Element not found or multiple elements found for locator: " + locator);
        }

        public IEnumerable<IWebElement> FindSeveral(string locator)
        {
            Log.Debug("Find several elements by locator: {Locator}", locator);
            return this.webDriver.FindElements(By.XPath(locator));
        }

        #endregion

        internal Browser(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
        }

        private readonly IWebDriver webDriver;
    }
}