namespace Tiver.Fowl.Core.Configuration
{
    using System;
    using System.Configuration;
    using Exceptions;

    public class ApplicationConfigurationSection : ConfigurationSection, IApplicationConfiguration
    {
        public string Title => TitleElement;

        public Uri StartUrl
        {
            get
            {
                Uri result;
                var validUriInConfiguration = Uri.TryCreate(StartUrlElement, UriKind.Absolute, out result);
                if (!validUriInConfiguration)
                {
                    throw new IncorrectApplicationConfigurationException("Invalid Uri in configuration.");
                }

                return result;
            }
        }

        [ConfigurationProperty("title", IsRequired = true)]
        private string TitleElement
        {
            get
            {
                return (string)this["title"];
            }

            set
            {
                this["title"] = value;
            }
        }

        [ConfigurationProperty("startUrl", IsRequired = true)]
        private string StartUrlElement
        {
            get
            {
                return (string)this["startUrl"];
            }

            set
            {
                this["startUrl"] = value;
            }
        }
    }
}
