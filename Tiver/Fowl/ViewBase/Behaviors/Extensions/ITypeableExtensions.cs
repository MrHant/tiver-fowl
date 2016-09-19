namespace Tiver.Fowl.ViewBase.Behaviors.Extensions
{
    using Core.Context;
    using Core.Exceptions;
    using Logging;
    using OpenQA.Selenium;

    public static class ITypeableExtensions
    {
        public static void Type(this ITypeable element, string value)
        {
            element.LogAction($"Type value '{value}'");
            element.Process(e => e.SendKeys(value));
        }

        public static void SetValue(this ITypeable element, string value)
        {
            element.Process(
                e => TestExecutionContext.Browser.BrowserActions.ExecuteScript($"arguments[0].setAttribute('value', '{value}')", e));
        }

        public static void PressEnter(this ITypeable element)
        {
            element.LogAction($"Press key 'Enter'");
            element.Process(e => e.SendKeys(Keys.Enter));
        }

        public static bool Enabled(this ITypeable element)
        {
            element.LogAction("Check whether element is enabled");
            return element.Process(e => e.Enabled);
        }
    }
}
