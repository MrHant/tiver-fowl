namespace Tiver.Fowl.Core.Configuration
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationMapper
    {
        public static ApplicationConfiguration Application = new ApplicationConfiguration();
        public static BrowserConfiguration Browser = new BrowserConfiguration();

        static ConfigurationMapper()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("Tiver_config.json", optional: true)
                .Build();

            config.GetSection("BrowserConfiguration").Bind(Browser);
            config.GetSection("ApplicationConfiguration").Bind(Application);
        }
    }
}