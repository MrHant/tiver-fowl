namespace Tiver.Fowl.WebDriverExtended.Browsers
{
    using System.Linq;
    using Contracts.Browsers;
    using OpenQA.Selenium;

    public abstract class Browser : IBrowser
    {
        private IWebDriver webDriver;

        internal Browser(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
        }

        public void Quit()
        {
            if (this.webDriver != null)
            {
                this.webDriver.Quit();
            }
        }

        public IWebElement Find(string locator)
        {
            var elements = this.webDriver.FindElements(By.XPath(locator));
            if (elements.Count == 1)
            {
                return elements.Single();
            }
            throw new NoSuchElementException();
        }
    }
}