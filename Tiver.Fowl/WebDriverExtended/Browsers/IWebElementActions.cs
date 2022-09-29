namespace Tiver.Fowl.WebDriverExtended.Browsers
{
    using System.Collections.Generic;
    using OpenQA.Selenium;

    public interface IWebElementActions
    {
        /// <summary>
        /// Find an element on page
        /// It's unsafe to call this method - can throw Stale exceptions. To be used only inside of Wait.Until
        /// </summary>
        /// <remarks>
        /// It's different from original Selenium method - it will fail in case more than one element found
        /// </remarks>
        /// <param name="locator">XPath locator to look for</param>
        /// <returns>Single element</returns>
        IWebElement Find(string locator);

        /// <summary>
        /// Find several elements on page
        /// It's unsafe to call this method - can throw Stale exceptions. To be used only inside of Wait.Until
        /// </summary>
        /// <param name="locator">XPath locator to look for</param>
        /// <returns>Collection of elements</returns>
        IEnumerable<IWebElement> FindSeveral(string locator);
    }
}
