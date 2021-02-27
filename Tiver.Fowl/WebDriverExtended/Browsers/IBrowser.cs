namespace Tiver.Fowl.WebDriverExtended.Browsers
{
    public interface IBrowser
    {
        IBrowserActions BrowserActions
        {
            get;
        }

        IWebElementActions WebElementActions
        {
            get;
        }
    }
}
