namespace Tiver.Fowl.WebDriverExtended.Configuration
{
    using System.Configuration;
    using Contracts.Configuration;

    public class ResolutionElement : ConfigurationElement, IResolution
    {
        [ConfigurationProperty("width", IsRequired = false)]
        public int? Width
        {
            get => (int?)this["width"];
            set => this["width"] = value;
        }

        [ConfigurationProperty("height", IsRequired = false)]
        public int? Height
        {
            get => (int?)this["height"];
            set => this["height"] = value;
        }
    }
}
