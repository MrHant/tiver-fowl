namespace Tiver.Fowl.WebDriverExtended.Contracts.Configuration
{
    using System;

    public interface IBrowserConfiguration
    {
        bool DownloadBinary { get; }

        string BrowserType { get; }

        Uri RemoteAddress { get; }
        
        IResolution Resolution { get; }
    }
}
