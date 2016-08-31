namespace Tiver.Fowl.WebDriverExtended.Browsers
{
    using System.Configuration;
    using System.Linq;
    using Contracts.Browsers;
    using Core.Configuration;
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
            this.webDriver?.Quit();
        }

        /// <summary>
        /// Find an element on page
        /// </summary>
        /// <remarks>
        /// It's different from original Selenium method - it will fail in case more than one element found
        /// </remarks>
        /// <param name="locator"></param>
        /// <returns></returns>
        public IWebElement Find(string locator)
        {
            var elements = this.webDriver.FindElements(By.XPath(locator));
            if (elements.Count == 1)
            {
                return elements.Single();
            }
            throw new NoSuchElementException();
        }

        /// <summary>
        /// Navigates to Start Uri defined in configuration
        /// </summary>
        public void NavigateToStartUri()
        {
            IApplicationConfiguration config = (ApplicationConfigurationSection)ConfigurationManager.GetSection("applicationConfigurationGroup/applicationConfiguration");
            webDriver.Navigate().GoToUrl(config.StartUrl);
        }
    }
}