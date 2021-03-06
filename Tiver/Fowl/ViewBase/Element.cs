﻿namespace Tiver.Fowl.ViewBase
{
    using System;
    using System.Configuration;
    using Behaviors;
    using Core.Configuration;
    using Core.Context;
    using Logging;
    using OpenQA.Selenium;
    using Waiting;
    using Waiting.Configuration;

    public class Element : IElement, INamed, IVisible, IClickable, IHasAttributes
    {
        /// <summary>
        /// Initialize an element
        /// </summary>
        /// <param name="locator">XPath locator of element</param>
        /// <param name="name">Verbose name of element for log file</param>
        public Element(string locator, string name=null)
        {
            Locator = locator;
            Name = name;
        }

        public TResult Process<TResult>(Func<IWebElement, TResult> function)
        {
            var result = default(TResult);
            Wait.Until(() =>
            {
                result = function.Invoke(this.WebElement);
                return true;
            }, (WaitConfigurationSection)ConfigurationManager.GetSection(ConfigurationSectionNames.Wait));

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

        public string Locator
        {
            get;
        }

        public string Name
        {
            get;
        }

        private IWebElement WebElement => TestExecutionContext.WebElementActions.Find(this.Locator);
    }
}
