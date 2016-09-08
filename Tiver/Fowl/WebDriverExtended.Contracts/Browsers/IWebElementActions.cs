namespace Tiver.Fowl.WebDriverExtended.Contracts.Browsers
{
    using OpenQA.Selenium;

    public interface IWebElementActions
    {
        /// <summary>
        /// Find an element on page
        /// </summary>
        /// <remarks>
        /// It's different from original Selenium method - it will fail in case more than one element found
        /// </remarks>
        /// <param name="locator"></param>
        /// <returns></returns>
        IWebElement Find(string locator);
    }
}
