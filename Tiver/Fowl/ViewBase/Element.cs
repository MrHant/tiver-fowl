namespace Tiver.Fowl.ViewBase
{
    using System;
    using Contracts;
    using Core;
    using Core.Context;
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

        public bool Displayed()
        {
            LogAction("Check whether element is displayed");
            return Process(e => e.Displayed);
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

        public void LogAction(string message)
        {
            Log.ForContext("LogType", "ElementAction").Information($"{{name}} :: {message}", name);
        }

        private IWebElement WebElement => TestExecutionContext.Browser.Find(this.locator);

        private readonly string locator;
        private readonly string name;
    }
}
