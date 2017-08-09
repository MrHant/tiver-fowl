namespace Tiver.Fowl.WebDriverExtended.Contracts.Configuration
{
    public interface IBrowserConfiguration
    {
        bool DownloadBinary { get; }

        string BrowserType { get; }

        IResolution Resolution { get; }
    }
}
