namespace Tiver.Fowl.WebDriverExtended.Browsers
{
    using System;
    using System.Configuration;
    using System.Linq;
    using Configuration;
    using Contracts.Configuration;
    using Drivers.Configuration;
    using Drivers.Downloaders;
    using Exceptions;
    using Serilog;

    public abstract class BrowserFactory
    {
        public static BrowserFactory GetFactory(string browserType)
        {
            Log.Information("Building instance of browser type '{browserType}'", browserType);

            switch (browserType)
            {
                // default browser type
                case null:
                case "":

                // other specific values
                case "firefox":
                    return new FirefoxBrowserFactory();

                case "chrome":
                    return new ChromeBrowserFactory();

                default:
                    throw new IncorrectBrowserConfigurationException(string.Format("Unsupported browser type: '{0}'.", browserType));
            }
        }

        public static Browser GetBrowser()
        {
            IBrowserConfiguration config = (BrowserConfigurationSection)ConfigurationManager.GetSection("browserConfigurationGroup/browserConfiguration");
            if (config.DownloadBinary)
            {
                var driverConfig = 
                    ((IDriversConfiguration)ConfigurationManager.GetSection("driversConfigurationGroup/driversConfiguration"))
                        .Instances.Cast<DriverElement>().Single(d => d.Name.Equals("chrome"));
                var downloader = (IDriverDownloader) Activator.CreateInstance(
                        "Tiver.Fowl.Drivers",
                        $"Tiver.Fowl.Drivers.Downloaders.{driverConfig.DownloaderType}")
                    .Unwrap();

                var uri = downloader.GetLinkForVersion(driverConfig.Version);
                var result = downloader.DownloadBinary(uri);
                if (!result)
                {
                    throw new Exception("Browser was not downloaded");
                }
            }

            var factory = GetFactory(config.BrowserType);
            return factory.Build(config);
        }

        public abstract Browser Build(IBrowserConfiguration configuration);
    }
}
