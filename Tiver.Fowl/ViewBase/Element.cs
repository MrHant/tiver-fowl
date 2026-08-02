namespace Tiver.Fowl.ViewBase
{
    using System;
    using System.Runtime.CompilerServices;
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
        /// to <see cref="Process"/> and <see cref="Process{TResult}"/> methods.
        /// Formatting can be also applied via <paramref name="locatorFormattingArguments"/>
        /// </remarks>
        /// <param name="locator">XPath locator of element</param>
        /// <param name="name">Verbose name of element for logging. </param>
        /// <param name="locatorFormattingArguments">Parameters which will be used for formatting locator</param>
        public Element(string locator, string name = "unnamed",  params object[] locatorFormattingArguments)
        {
            Locator = locator;
            Name = name;
            LocatorFormattingArguments = locatorFormattingArguments;
        }

        /// <summary>
        /// Initialize an element from another element (copies locator)
        /// </summary>
        /// <param name="source">Source element to copy locator from</param>
        /// <param name="name"></param>
        /// <param name="locatorFormattingArguments"></param>
        public Element(IElement source, string name = "unnamed", params object[] locatorFormattingArguments)
        : this(source.Locator, name, locatorFormattingArguments)
        {
        }
        
        public TResult Process<TResult>(Func<IWebElement, TResult> function, params object[] locatorFormattingArguments)
        {
            // Wait.Until either runs the callback to completion or throws, so result is assigned by
            // the time we return. The compiler cannot see that through the delegate, hence default!.
            TResult result = default!;
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

        public object[] LocatorFormattingArguments
        {
            get;
            set;
        } = Array.Empty<object>();

        public string Name
        {
            get;
        }

        private IWebElement GetWebElement(params object[] locatorFormattingArguments)
        {
            string? locator = null;
            try
            {
                var arguments = locatorFormattingArguments.Length > 0
                    ? locatorFormattingArguments
                    : LocatorFormattingArguments;
                locator = string.Format(this.Locator, arguments);
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
