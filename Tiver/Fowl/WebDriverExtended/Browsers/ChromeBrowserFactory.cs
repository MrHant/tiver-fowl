namespace Tiver.Fowl.WebDriverExtended.Browsers
{
    using System;
    using System.Drawing;
    using Contracts.Configuration;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    public class ChromeBrowserFactory : BrowserFactory
    {
        public override Browser Build(IBrowserConfiguration configuration)
        {
            IWebDriver driver = new ChromeDriver();

            if (configuration.Resolution.Width != null || configuration.Resolution.Height != null)
            {
                int width = Convert.ToInt32(configuration.Resolution.Width);
                int height = Convert.ToInt32(configuration.Resolution.Height);
                driver.Manage().Window.Size = new Size(width, height);
            }

            return new ChromeBrowser(driver);
        }
    }
}
