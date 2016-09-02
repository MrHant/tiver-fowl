namespace Tiver.Fowl.WebDriverExtended.Browsers
{
    using System.Configuration;
    using Configuration;
    using Contracts.Configuration;
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
                case "ff":
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
            var factory = GetFactory(config.BrowserType);
            return factory.Build(config);
        }

        public abstract Browser Build(IBrowserConfiguration configuration);
    }
}
