namespace Tiver.Fowl.Core.Configuration
{
    using System;

    public class BrowserConfiguration 
    {
        public bool DownloadBinary { get; set; }

        public string BrowserType { get; set; }

        public Uri RemoteAddress { get; set; }

        public Resolution Resolution { get; set; }
    }
}
