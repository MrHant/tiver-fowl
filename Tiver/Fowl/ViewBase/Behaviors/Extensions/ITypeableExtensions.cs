namespace Tiver.Fowl.ViewBase.Behaviors.Extensions
{
    using Core.Context;
    using Core.Exceptions;
    using Logging;
    using OpenQA.Selenium;

    public static class ITypeableExtensions
    {
        public static void Type(this ITypeable element, string value, params object[] locatorFormattingArguments)
        {
            element.LogAction($"Type value '{value}'");
            element.Process(
                e => e.SendKeys(value),
                locatorFormattingArguments
            );
        }

        public static void SetValue(this ITypeable element, string value, params object[] locatorFormattingArguments)
        {
            element.Process(
                e => TestExecutionContext.BrowserActions
                    .ExecuteScript($"arguments[0].setAttribute('value', '{value}')", e),
                locatorFormattingArguments
            );
        }

        public static void PressEnter(this ITypeable element, params object[] locatorFormattingArguments)
        {
            element.LogAction($"Press key 'Enter'");
            element.Process(
                e => e.SendKeys(Keys.Enter),
                locatorFormattingArguments
            );
        }

        public static bool Enabled(this ITypeable element, params object[] locatorFormattingArguments)
        {
            element.LogAction("Check whether element is enabled");
            return element.Process(
                e => e.Enabled,
                locatorFormattingArguments
            );
        }
    }
}
