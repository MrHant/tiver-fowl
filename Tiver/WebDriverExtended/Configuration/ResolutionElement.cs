namespace Tiver.WebDriverExtended.Configuration
{
    using System.Configuration;

    public class ResolutionElement : ConfigurationElement, IResolution
    {
        [ConfigurationProperty("width", IsRequired = false)]
        public int? Width
        {
            get
            {
                return (int?)this["width"];
            }

            set
            {
                this["width"] = value;
            }
        }

        [ConfigurationProperty("height", IsRequired = false)]
        public int? Height
        {
            get
            {
                return (int?)this["height"];
            }

            set
            {
                this["height"] = value;
            }
        }
    }
}
