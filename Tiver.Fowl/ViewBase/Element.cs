namespace Tiver.Fowl.ViewBase
{
    using System;
    using Behaviors;
    using Core.Context;
    using Exceptions;
    using Logging;
    using OpenQA.Selenium;
    using Waiting;

    public class Element : IElement, INamed, IVisible, IClickable, IHasAttributes
    {
        /// <summary>
        /// Initialize an element
        /// </summary>
        /// <remarks>
        /// <paramref name="locator"/> can include formatting places, to be later replaced with arguments passed
        /// to <see cref="Process"/> and <see cref="Process{TResult}"/> methods
        /// </remarks>
        /// <param name="locator">XPath locator of element</param>
        /// <param name="name">Verbose name of element for log file</param>
        public Element(string locator, string name = null)
        {
            Locator = locator;
            Name = name;
        }

        public TResult Process<TResult>(Func<IWebElement, TResult> function, params object[] locatorFormattingArguments)
        {
            var result = default(TResult);
            Wait.Until(() =>
            {
                result = function.Invoke(GetWebElement(locatorFormattingArguments));
                return true;
            });

            return result;
        }

        public void Process(Action<IWebElement> action, params object[] locatorFormattingArguments)
        {
            Process(e =>
                {
                    action.Invoke(e);
                    return true;
                },
                locatorFormattingArguments);
        }

        public string Locator
        {
            get;
        }

        public string Name
        {
            get;
        }

        private IWebElement GetWebElement(params object[] locatorFormattingArguments)
        {
            string locator = null;
            try
            {
                locator = string.Format(this.Locator, locatorFormattingArguments);
            }
            catch (FormatException formatException)
            {
                throw new LocatorFormattingException(
                    $"Error during locator formatting. Please ensure all required arguments are passed " +
                    $"for formatting. Locator: [{this.Locator}], Arguments: [{locatorFormattingArguments}]",
                    formatException
                );
            }
            
            return TestExecutionContext.WebElementActions.Find(locator);
        }
    }
}
