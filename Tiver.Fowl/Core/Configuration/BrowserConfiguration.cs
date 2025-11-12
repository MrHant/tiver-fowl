using System;

namespace Tiver.Fowl.Core.Configuration
{
    public class BrowserConfiguration
    {
        /// <summary>
        /// Specifies how browser drivers should be managed.
        /// Default is SeleniumManager, which uses Selenium's built-in driver management.
        /// </summary>
        public DriverManagerType? DriverManager { get; set; }

        public string BrowserType { get; set; }

        public bool Headless { get; set; }

        public bool RunningInDocker { get; set; } = false;

        public Uri RemoteAddress { get; set; }

        public Resolution Resolution { get; set; }
    }
}
