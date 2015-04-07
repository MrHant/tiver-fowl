namespace Tiver.WebDriverExtended.Browsers
{
    using OpenQA.Selenium;

    public abstract class Browser
    {
        private IWebDriver webDriver;

        internal Browser(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
        }

        public void Quit()
        {
            if (this.webDriver != null)
            {
                this.webDriver.Quit();
            }
        }
    }
}