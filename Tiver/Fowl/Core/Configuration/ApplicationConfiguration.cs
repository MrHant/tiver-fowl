namespace Tiver.Fowl.Core.Configuration
{
    using System;

    public class ApplicationConfiguration : IApplicationConfiguration
    {
        public ApplicationConfiguration(string title, Uri startUrl)
        {
            this.Title = title;
            this.StartUrl = startUrl;   
        }

        public string Title { get; private set; }

        public Uri StartUrl { get; private set; }
    }
}
