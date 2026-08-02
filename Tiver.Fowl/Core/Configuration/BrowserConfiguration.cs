namespace Tiver.Fowl.Core.Configuration
{
    using System;

    public class BrowserConfiguration
    {
        /// <summary>
        /// Specifies how browser drivers should be managed.
        /// Default is SeleniumManager, which uses Selenium's built-in driver management.
        /// </summary>
        public DriverManagerType? DriverManager { get; set; }

        /// <summary>
        /// Browser to launch. Null or empty selects the default browser, as
        /// <see cref="Browsers.BrowserFactory.GetFactory"/> treats both as unspecified.
        /// </summary>
        public string? BrowserType { get; set; }

        public bool Headless { get; set; }

        public bool RunningInDocker { get; set; } = false;

        /// <summary>
        /// Address of a remote Selenium grid. Null runs the browser locally.
        /// </summary>
        public Uri? RemoteAddress { get; set; }

        /// <summary>
        /// Window size to apply after launch. Null leaves the browser at its default size.
        /// </summary>
        public Resolution? Resolution { get; set; }
    }
}
