namespace Tiver.Fowl.ViewBase
{
    using System;
    using OpenQA.Selenium;

    public interface IElement
    {
        /// <summary>
        /// Process some Selenium specific actions
        /// </summary>
        /// <typeparam name="TResult">type of function's result</typeparam>
        /// <param name="function">action to be performed</param>
        /// <param name="locatorFormattingArguments">arguments to be used for element's locator</param>
        /// <returns>Evaluation result of <paramref name="function"/></returns>
        TResult Process<TResult>(Func<IWebElement, TResult> function, params object[] locatorFormattingArguments);

        /// <summary>
        /// Process some Selenium specific actions
        /// </summary>
        /// <param name="action">action to be performed</param>
        /// <param name="locatorFormattingArguments">arguments to be used for element's locator</param>
        /// <returns>Evaluation result of <paramref name="action"/></returns>
        void Process(Action<IWebElement> action, params object[] locatorFormattingArguments);

        /// <summary>
        /// Locator of element
        /// </summary>
        /// <remarks>
        /// Can include formatting places, to be later replaced with arguments passed to <see cref="Process"/> and <see cref="Process{TResult}"/> methods
        /// </remarks>
        string Locator { get; }
    }
}
