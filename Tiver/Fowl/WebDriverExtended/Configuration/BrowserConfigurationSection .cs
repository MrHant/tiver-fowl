namespace Tiver.Fowl.WebDriverExtended.Configuration
{
    using System;
    using System.Configuration;
    using Contracts.Configuration;

    public class BrowserConfigurationSection : IBrowserConfiguration
    {

        public bool DownloadBinary { get; set; }

        public string BrowserType { get; set; }

        public Uri RemoteAddress { get; set; }

        public IResolution Resolution { get; set; }
    }
}
