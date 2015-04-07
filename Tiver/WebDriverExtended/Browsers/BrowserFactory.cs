namespace Tiver.WebDriverExtended.Browsers
{
    using Tiver.WebDriverExtended.Configuration;
    using Tiver.WebDriverExtended.Exceptions;

    public abstract class BrowserFactory
    {
        public static BrowserFactory GetFactory(string browserType)
        {
            switch (browserType)
            {
                // default browser type
                case null:
                case "":

                // other specific values
                case "ff":
                case "firefox":
                    return new FirefoxBrowserFactory();

                default:
                    throw new IncorrectBrowserConfigurationException(string.Format("Unsupported browser type: '{0}'.", browserType));
            }
        }

        public static Browser GetBrowser()
        {
            BrowserConfigurationSection config = (BrowserConfigurationSection)System.Configuration.ConfigurationManager.GetSection("browserConfigurationGroup/browserConfiguration");
            var factory = GetFactory(config.BrowserType);
            return factory.Build(config);
        }

        public abstract Browser Build(IBrowserConfiguration configuration);
    }
}
