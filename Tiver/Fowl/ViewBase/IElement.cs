namespace Tiver.Fowl.ViewBase
{
    using System;
    using OpenQA.Selenium;

    public interface IElement
    {
        /// <summary>
        /// Process some Selenium specific actions
        /// </summary>
        /// <typeparam name="TResult">type of funtion's result</typeparam>
        /// <param name="function">action to be performed</param>
        /// <returns>Evaluation result of <paramref name="function"/></returns>
        TResult Process<TResult>(Func<IWebElement, TResult> function);

        /// <summary>
        /// Process some Selenium specific actions
        /// </summary>
        /// <param name="action">action to be performed</param>
        /// <returns>Evaluation result of <paramref name="action"/></returns>
        void Process(Action<IWebElement> action);

        /// <summary>
        /// Locator of element
        /// </summary>
        string Locator { get; }
    }
}
