namespace Tiver.Fowl.WebDriverExtended.Browsers
{
    using System;

    public interface IBrowserActions
    {
        void NavigateToUrl(Uri url);

        void Refresh();

        void Back();

        void Forward();
        
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

        void CloseWindow();

        void Quit();
    }
}
