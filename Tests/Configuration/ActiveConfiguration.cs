namespace Tests.Configuration
{
    using System;
    using Microsoft.Extensions.Configuration;

    public static class ActiveConfiguration
    {
        private static readonly IConfigurationRoot Config;

        static ActiveConfiguration()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("config.json", optional: true)
                .Build();
        }

        public static Uri StartUrl => new(Config.GetValue<string>("StartUrl"));
    }
}