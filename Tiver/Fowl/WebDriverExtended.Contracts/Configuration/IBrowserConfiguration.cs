namespace Tiver.Fowl.WebDriverExtended.Contracts.Configuration
{
    public interface IBrowserConfiguration
    {
        string BrowserType { get; }

        IResolution Resolution { get; }
    }
}
