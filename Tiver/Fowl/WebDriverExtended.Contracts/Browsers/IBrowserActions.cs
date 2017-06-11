namespace Tiver.Fowl.WebDriverExtended.Contracts.Browsers
{
    public interface IBrowserActions
    {
        /// <summary>
        /// Navigates to Start Uri defined in configuration
        /// </summary>
        void NavigateToStartUri();

        /// <summary>
        /// Switches to frame found by locator
        /// </summary>
        /// <param name="locator"></param>
        void SwitchToFrame(string locator);

        /// <summary>
        /// Switches back to main frame
        /// </summary>
        void SwitchToMainFrame();

        object ExecuteScript(string script, params object[] arguments);

        /// <summary>
        /// Takes screenshot and logs it as base64
        /// </summary>
        void TakeScreenshot();

        void Quit();
    }
}
