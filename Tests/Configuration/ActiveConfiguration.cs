namespace Tests.Configuration
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Tiver.Fowl.Core.Context;

    public static class ActiveConfiguration
    {
        private static readonly IConfigurationRoot Config;
        private static readonly UrlsConfiguration UrlsConfig;

        static ActiveConfiguration()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("config.json", optional: true)
                .Build();

            UrlsConfig = new UrlsConfiguration();
            Config.GetSection("Urls").Bind(UrlsConfig.Urls);
        }

        public static UrlsConfiguration Urls => UrlsConfig;

        public static void NavigateTo(string urlName)
        {
            var url = UrlsConfig.GetUrl(urlName);
            TestExecutionContext.BrowserActions.NavigateToUrl(url);
        }
    }
}
