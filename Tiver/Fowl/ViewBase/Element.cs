namespace Tiver.Fowl.ViewBase
{
    using System;
    using Contracts;
    using Core;
    using Core.Context;
    using Core.Exceptions;
    using OpenQA.Selenium;
    using Serilog;

    public class Element : IElement
    {
        /// <summary>
        /// Initialize an element
        /// </summary>
        /// <param name="locator">XPath locator of element</param>
        public Element(string locator)
        {
            this.locator = locator;
            this.name = "Unnamed";
        }

        /// <summary>
        /// Initialize an element
        /// </summary>
        /// <param name="locator">XPath locator of element</param>
        /// <param name="name">Verbose name of element for log file</param>
        public Element(string locator, string name)
        {
            this.locator = locator;
            this.name = name;
        }

        /// <summary>
        /// Checks whether element is displayed
        /// </summary>
        /// <remarks>
        /// Waits till timeout before returning false
        /// </remarks>
        /// <returns>true if element is displayed, false otherwise</returns>
        public bool Displayed()
        {
            LogAction("Check whether element is displayed");
            try
            {
                return Process(e => e.Displayed);
            }
            catch (WaitTimeoutException)
            {
                // If not found within timeout => not displayed
                return false;
            }
        }

        public void Click()
        {
            LogAction("Clicking");
            Process(e => e.Click());
        }

        public void SendKeys(string value)
        {
            LogAction($"Sending keys '{value}'");
            Process(e => e.SendKeys(value));
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

        public void LogAction(string action)
        {
            Log.ForContext("LogType", "ElementAction").Information($"{{Name}} :: {{Action}}", name, action);
        }

        private IWebElement WebElement => TestExecutionContext.Browser.WebElementActions.Find(this.locator);

        private readonly string locator;
        private readonly string name;
    }
}
