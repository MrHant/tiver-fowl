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
            IWebDriver driver;
            if (configuration.RemoteAddress != null)
            {
                var options = new FirefoxOptions();
                driver = new RemoteWebDriver(configuration.RemoteAddress, options);
            }
            else
            {
                driver = new FirefoxDriver();
            }

            if (configuration.Resolution.Width != null || configuration.Resolution.Height != null)
            {
                int width = Convert.ToInt32(configuration.Resolution.Width);
                int height = Convert.ToInt32(configuration.Resolution.Height);
                driver.Manage().Window.Size = new Size(width, height);
            }

            return new FirefoxBrowser(driver);
        }
    }
}
