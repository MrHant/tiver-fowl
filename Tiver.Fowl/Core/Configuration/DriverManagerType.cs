namespace Tiver.Fowl.Core.Configuration
{
    /// <summary>
    /// Specifies how browser drivers should be managed and downloaded.
    /// </summary>
    public enum DriverManagerType
    {
        /// <summary>
        /// Use Selenium Manager (default). Selenium automatically downloads and manages browser drivers.
        /// This is the recommended option as it's built into Selenium and requires no additional configuration.
        /// </summary>
        SeleniumManager,

        /// <summary>
        /// Use Tiver.Fowl.Drivers package for driver management.
        /// Provides more control over driver versions and download behavior.
        /// Requires configuration in "Tiver.Fowl.Drivers" section of Tiver_config.json.
        /// </summary>
        TiverFowlDrivers,

        /// <summary>
        /// No automatic driver management. Assumes drivers are already available in PATH.
        /// Use this if you manage drivers manually or through external tools.
        /// </summary>
        None
    }
}
