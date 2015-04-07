namespace Tiver.WebDriverExtended.Configuration
{
    public interface IBrowserConfiguration
    {
        string BrowserType { get; }

        IResolution Resolution { get; }
    }
}
