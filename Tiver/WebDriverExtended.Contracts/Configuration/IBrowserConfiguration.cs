namespace Tiver.WebDriverExtended.Contracts.Configuration
{
    public interface IBrowserConfiguration
    {
        string BrowserType { get; }

        IResolution Resolution { get; }
    }
}
