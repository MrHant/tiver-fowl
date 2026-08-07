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
            var options = BuildOptions(configuration);
            var windowSize = ResolveWindowSize(configuration.Resolution);

            IWebDriver driver = configuration.RemoteAddress != null
                ? new RemoteWebDriver(configuration.RemoteAddress, options)
                : new ChromeDriver(options);

            if (windowSize is not null)
            {
                driver.Manage().Window.Size = windowSize.Value;
            }

            return new ChromeBrowser(driver);
        }

        /// <summary>
        /// Builds the options describing *what* the browser should be. This is deliberately
        /// independent of *where* it runs: a grid session gets the same options as a local one,
        /// which is what makes headless-on-grid — the most common grid setup there is — behave.
        /// </summary>
        internal static ChromeOptions BuildOptions(BrowserConfiguration configuration)
        {
            var options = new ChromeOptions();

            if (configuration.Headless)
            {
                options.AddArgument("--headless");
            }

            if (UseDockerMode(configuration))
            {
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
            }

            return options;
        }

        /// <summary>
        /// Chrome needs <c>--no-sandbox</c> and <c>--disable-dev-shm-usage</c> to run inside a
        /// container. The two signals for that are not equivalent: <see
        /// cref="BrowserConfiguration.RunningInDocker"/> is a statement about the browser and holds
        /// wherever it runs, while the <c>/.dockerenv</c> probe only describes *this* process's
        /// machine. On a grid the browser runs on the node, so the local probe says nothing about
        /// it and is consulted only when launching locally — a grid node that needs the switches
        /// must be told via configuration.
        /// </summary>
        private static bool UseDockerMode(BrowserConfiguration configuration)
        {
            return configuration.RunningInDocker
                || (configuration.RemoteAddress is null && IsRunningInDocker());
        }
    }
}
