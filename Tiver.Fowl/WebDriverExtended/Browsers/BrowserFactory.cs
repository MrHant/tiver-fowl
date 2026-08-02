namespace Tiver.Fowl.WebDriverExtended.Browsers
{
    using System;
    using Core.Configuration;
    using Drivers;
    using Exceptions;
    using Serilog;

    public abstract class BrowserFactory
    {
        /// <summary>
        /// Browser used when <see cref="BrowserConfiguration.BrowserType"/> is null or empty.
        /// </summary>
        private const string DefaultBrowserType = "firefox";

        public static BrowserFactory GetFactory(string? browserType)
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
            var config = ConfigurationMapper.Browser;

            // GetFactory treats null and empty as "the default browser", but the driver downloader
            // needs a concrete name, so resolve it here rather than passing an unset value through.
            var browserType = string.IsNullOrEmpty(config.BrowserType)
                ? DefaultBrowserType
                : config.BrowserType;

            // Determine which driver manager to use
            var driverManager = GetDriverManagerType(config);

            // Handle driver management based on configuration
            switch (driverManager)
            {
                case DriverManagerType.TiverFowlDrivers:
                    Log.Information("Using Tiver.Fowl.Drivers for driver management");
                    var result = Downloaders.DownloadBinaryFor(browserType);
                    if (!result.Successful)
                    {
                        throw new Exception($"Driver download failed for browser type '{browserType}'");
                    }
                    break;

                case DriverManagerType.SeleniumManager:
                    Log.Information("Using Selenium Manager for driver management (automatic)");
                    // Do nothing - Selenium Manager handles this automatically
                    break;

                case DriverManagerType.None:
                    Log.Information("No automatic driver management - assuming drivers are in PATH");
                    // Do nothing - assume drivers are already available
                    break;
            }

            var factory = GetFactory(browserType);
            return factory.Build(config);
        }

        private static DriverManagerType GetDriverManagerType(BrowserConfiguration config)
        {
            // Return the configured value, or default to SeleniumManager
            return config.DriverManager ?? DriverManagerType.SeleniumManager;
        }

        public abstract Browser Build(BrowserConfiguration configuration);
    }
}
