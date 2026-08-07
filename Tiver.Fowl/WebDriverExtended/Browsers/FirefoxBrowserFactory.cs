namespace Tiver.Fowl.WebDriverExtended.Browsers
{
    using System;
    using System.Drawing;
    using Core.Configuration;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.Remote;

    public class FirefoxBrowserFactory : BrowserFactory
    {
        public override Browser Build(BrowserConfiguration configuration)
        {
            var options = BuildOptions(configuration);

            IWebDriver driver = configuration.RemoteAddress != null
                ? new RemoteWebDriver(configuration.RemoteAddress, options)
                : new FirefoxDriver(options);

            if (configuration.Resolution?.Width != null || configuration.Resolution?.Height != null)
            {
                int width = Convert.ToInt32(configuration.Resolution.Width);
                int height = Convert.ToInt32(configuration.Resolution.Height);
                driver.Manage().Window.Size = new Size(width, height);
            }

            return new FirefoxBrowser(driver);
        }

        /// <summary>
        /// Builds the options describing *what* the browser should be. This is deliberately
        /// independent of *where* it runs: a grid session gets the same options as a local one,
        /// which is what makes headless-on-grid — the most common grid setup there is — behave.
        /// </summary>
        internal static FirefoxOptions BuildOptions(BrowserConfiguration configuration)
        {
            var options = new FirefoxOptions();

            if (configuration.Headless)
            {
                options.AddArgument("-headless");
            }

            return options;
        }
    }
}
