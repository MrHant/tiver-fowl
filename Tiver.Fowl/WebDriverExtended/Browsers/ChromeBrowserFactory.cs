namespace Tiver.Fowl.WebDriverExtended.Browsers
{
    using System;
    using System.Drawing;
    using System.IO;
    using Core.Configuration;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Remote;

    public class ChromeBrowserFactory : BrowserFactory
    {
        private static bool IsRunningInDocker()
        {
            // Check for .dockerenv file (most common indicator)
            if (File.Exists("/.dockerenv"))
            {
                return true;
            }

            return false;
        }

        public override Browser Build(BrowserConfiguration configuration)
        {
            IWebDriver driver;
            if (configuration.RemoteAddress != null)
            {
                var options = new ChromeOptions();
                driver = new RemoteWebDriver(configuration.RemoteAddress, options);
            }
            else
            {
                var options = new ChromeOptions();
                if (configuration.Headless)
                {
                    options.AddArgument("--headless");
                }

                bool useDockerMode = configuration.RunningInDocker || IsRunningInDocker();
                if (useDockerMode)
                {
                    options.AddArgument("--no-sandbox");
                    options.AddArgument("--disable-dev-shm-usage");
                }

                driver = new ChromeDriver(options);
            }

            if (configuration.Resolution?.Width != null || configuration.Resolution?.Height != null)
            {
                int width = Convert.ToInt32(configuration.Resolution.Width);
                int height = Convert.ToInt32(configuration.Resolution.Height);
                driver.Manage().Window.Size = new Size(width, height);
            }

            return new ChromeBrowser(driver);
        }
    }
}
