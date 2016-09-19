namespace Tiver.Fowl.ViewBase
{
    using System;
    using Behaviors;
    using Core;
    using Core.Context;
    using Logging;
    using OpenQA.Selenium;

    public class Element : IElement, INamed, IVisible, IClickable
    {
        /// <summary>
        /// Initialize an element
        /// </summary>
        /// <param name="locator">XPath locator of element</param>
        public Element(string locator)
        {
            this.locator = locator;
            this.Name = "Unnamed";
        }

        /// <summary>
        /// Initialize an element
        /// </summary>
        /// <param name="locator">XPath locator of element</param>
        /// <param name="name">Verbose name of element for log file</param>
        public Element(string locator, string name)
        {
            this.locator = locator;
            this.Name = name;
        }

        public TResult Process<TResult>(Func<IWebElement, TResult> function)
        {
            var result = default(TResult);
            Wait.Until(() =>
            {
                result = function.Invoke(this.WebElement);
                return true;
            }, typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            return result;
        }

        public void Process(Action<IWebElement> action)
        {
            Process(e =>
            {
                action.Invoke(e);
                return true;
            });
        }

        public string Name
        {
            get;
        }

        private IWebElement WebElement => TestExecutionContext.Browser.WebElementActions.Find(this.locator);

        private readonly string locator;
    }
}
