namespace Tiver.Fowl.WebDriverExtended.Browsers
{
    using System;
    using Core.Configuration;
    using Drivers;
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
            var config = ConfigurationMapper.Browser;
            var browserType = config.BrowserType;
            if (config.DownloadBinary)
            {
                var result = Downloaders.DownloadBinaryFor(browserType);
                if (!result.Successful)
                {
                    throw new Exception("Browser was not downloaded");
                }
            }

            var factory = GetFactory(config.BrowserType);
            return factory.Build(config);
        }

        public abstract Browser Build(BrowserConfiguration configuration);
    }
}
