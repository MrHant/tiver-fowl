namespace Tiver.Fowl.ViewBase.Behaviors.Extensions
{
    using Core.Exceptions;
    using Logging;

    public static class IVisibleExtensions
    {
        /// <summary>
        /// Checks whether element is displayed
        /// </summary>
        /// <remarks>
        /// Waits till timeout before returning false
        /// </remarks>
        /// <returns>true if element is displayed, false otherwise</returns>
        public static bool Displayed(this IVisible element)
        {
            element.LogAction("Check whether element is displayed");
            try
            {
                return element.Process(e => e.Displayed);
            }
            catch (WaitTimeoutException)
            {
                // If not found within timeout => not displayed
                return false;
            }
        }
    }
}