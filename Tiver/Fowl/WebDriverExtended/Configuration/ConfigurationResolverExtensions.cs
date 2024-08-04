namespace Tiver.Fowl.WebDriverExtended.Configuration
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Tiver.Fowl.Core.Configuration;
    using Tiver.Fowl.WebDriverExtended.Contracts.Configuration;

    public static class ConfigurationResolverExtensions
    {
        public static BrowserConfiguration GetBrowserConfigurationFromFile(this ConfigurationResolver resolver)
        {
            var browserConfiguration = new BrowserConfiguration();

            var config = new ConfigurationBuilder()
                .AddJsonFile("Tiver_config.json", optional: true)
                .Build();
            config.GetSection("Tiver.Fowl").Bind(browserConfiguration);
            return browserConfiguration;

        }
    }
}
