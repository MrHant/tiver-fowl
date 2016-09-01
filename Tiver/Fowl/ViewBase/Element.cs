namespace Tiver.Fowl.ViewBase
{
    using System;
    using Contracts;
    using Core.Context;
    using OpenQA.Selenium;

    public class Element : IElement
    {
        /// <summary>
        /// Initialize an element
        /// </summary>
        /// <param name="locator">XPath locator of element</param>
        public Element(string locator)
        {
            this.locator = locator;
        }

        public TResult Process<TResult>(Func<IWebElement, TResult> function)
        {
            var result = function.Invoke(this.WebElement);
            return result;
        }

        public void Process(Action<IWebElement> action)
        {
            action.Invoke(this.WebElement);
        }

        private IWebElement WebElement => TestExecutionContext.Browser.Find(this.locator);

        private readonly string locator;
    }
}
