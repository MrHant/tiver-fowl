namespace Tiver.Fowl.Core.Configuration
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Tiver.Fowl.Waiting.Configuration;

    public class ConfigurationResolver
    {
        public CoreConfiguration GetConfigurationFromFile()
        {
            var coreConfiguration = new CoreConfiguration();

            var config = new ConfigurationBuilder()
                .AddJsonFile("Tiver_config.json", optional: true)
                .Build();
            config.GetSection("Tiver.Fowl").Bind(coreConfiguration);
            return coreConfiguration;
        }
    }
}
