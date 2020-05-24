namespace Tiver.Fowl.ViewBase.Behaviors.Extensions
{
    using Logging;

    public static class IClickableExtensions
    {
        public static void Click(this IClickable element, params object[] locatorFormattingArguments)
        {
            element.LogAction("Clicking");
            element.Process(
                e => e.Click(),
                locatorFormattingArguments
            );
        }
    }
}