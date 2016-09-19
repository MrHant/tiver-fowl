namespace Tiver.Fowl.ViewBase.Behaviors.Extensions
{
    using Logging;

    public static class IClickableExtensions
    {
        public static void Click(this IClickable element)
        {
            element.LogAction("Clicking");
            element.Process(e => e.Click());
        }
    }
}