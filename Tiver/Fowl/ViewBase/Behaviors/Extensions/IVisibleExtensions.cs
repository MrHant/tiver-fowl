namespace Tiver.Fowl.ViewBase.Behaviors.Extensions
{
    using Logging;
    using Waiting.Exceptions;

    public static class IVisibleExtensions
    {
        /// <summary>
        /// Checks whether element is displayed
        /// </summary>
        /// <exception cref="WaitTimeoutException">Thrown in case not able to get Displayed result in within timeout</exception>
        /// <returns>true if element is displayed, false otherwise</returns>
        public static bool Displayed(this IVisible element, params object[] locatorFormattingArguments)
        {
            element.LogAction("Check whether element is displayed");
            var result = false;
            element.Process(
                e =>
                {
                    result = e.Displayed;
                    return true;
                },
                locatorFormattingArguments
            );
            return result;
        }
    }
}