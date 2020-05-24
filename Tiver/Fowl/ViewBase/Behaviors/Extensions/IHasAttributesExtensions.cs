namespace Tiver.Fowl.ViewBase.Behaviors.Extensions
{
    using Logging;

    public static class IHasAttrbutesExtensions
    {
        public static string GetAttribute(this IHasAttributes element, string attributeName, params object[] locatorFormattingArguments)
        {
            element.LogAction($"Getting attribute value for '{attributeName}'");
            var result = string.Empty;
            element.Process(
                e =>
                {
                    result = e.GetAttribute(attributeName);
                    return true;
                },
                locatorFormattingArguments
            );
            return result;
        }
    }
}