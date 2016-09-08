namespace Tiver.Fowl.WebDriverExtended.Contracts.Browsers
{
    public interface IBrowserActions
    {
        /// <summary>
        /// Navigates to Start Uri defined in configuration
        /// </summary>
        void NavigateToStartUri();

        object ExecuteScript(string script, params object[] arguments);

        /// <summary>
        /// Takes screenshot and logs it as base64
        /// </summary>
        void TakeScreenshot();

        void Quit();
    }
}
