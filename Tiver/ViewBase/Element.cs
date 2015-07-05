namespace Tiver.ViewBase
{
    using System;
    using OpenQA.Selenium;
    using Tiver.Core.Context;
    using Tiver.ViewBase.Contracts;
    using Tiver.WebDriverExtended.Browsers;

    internal abstract class Element : IElement
    {
        private readonly string locator;

        /// <summary>
        /// Initialize an element
        /// </summary>
        /// <param name="locator">XPath locator of element</param>
        public Element(string locator)
        {
            this.locator = locator;
        }

        private IWebElement WebElement
        {
            get
            {
                return TestExecutionContext.Browser.Find(this.locator);
            }
        }

        /// <summary>
        /// Process some Selenium specific actions
        /// </summary>
        /// <typeparam name="TResult">type of funtion's result</typeparam>
        /// <param name="function">action to be performed</param>
        /// <returns></returns>
        public TResult Process<TResult>(Func<IWebElement, TResult> function)
        {
            var result = default(TResult);
            result = function.Invoke(this.WebElement);
            return result;
        }
    }
}
