namespace Tiver.Fowl.WebDriverExtended.Browsers
{
    using System;
    using System.Configuration;
    using System.Linq;
    using Configuration;
    using Contracts.Configuration;
    using Drivers;
    using Drivers.Configuration;
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
            var browserType = config.BrowserType;
            if (config.DownloadBinary)
            {
                var driverConfig = 
                    ((IDriversConfiguration)ConfigurationManager.GetSection("driversConfigurationGroup/driversConfiguration"))
                        .Instances.Cast<DriverElement>().Single(d => d.Name.Equals(browserType));
                
                var result = Downloaders.Get(browserType).DownloadBinary(driverConfig.Version);
                if (!result.Successful)
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
