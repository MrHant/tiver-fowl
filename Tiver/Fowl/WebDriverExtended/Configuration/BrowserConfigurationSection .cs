namespace Tiver.Fowl.WebDriverExtended.Configuration
{
    using System.Configuration;
    using Contracts.Configuration;

    public class BrowserConfigurationSection : ConfigurationSection, IBrowserConfiguration
    {
        public IResolution Resolution => ResolutionElement;

        [ConfigurationProperty("downloadBinary", DefaultValue = false, IsRequired = false)]
        public bool DownloadBinary
        {
            get => (bool)this["downloadBinary"];
            set => this["downloadBinary"] = value;
        }

        [ConfigurationProperty("browserType", DefaultValue = null, IsRequired = false)]
        public string BrowserType
        {
            get => (string)this["browserType"];
            set => this["browserType"] = value;
        }

        [ConfigurationProperty("resolution", DefaultValue = null, IsRequired = false)]
        public ResolutionElement ResolutionElement
        {
            get => (ResolutionElement)this["resolution"];
            set => this["resolution"] = value;
        }
    }
}
