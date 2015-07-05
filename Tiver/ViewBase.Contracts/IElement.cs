namespace Tiver.ViewBase.Contracts
{
    using System;
    using OpenQA.Selenium;

    public interface IElement
    {
        TResult Process<TResult>(Func<IWebElement, TResult> function);
    }
}
