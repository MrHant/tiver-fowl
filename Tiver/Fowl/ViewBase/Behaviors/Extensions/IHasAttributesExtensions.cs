namespace Tiver.Fowl.ViewBase.Behaviors.Extensions
{
    using Logging;

    public static class IHasAttrbutesExtensions
    {
        public static void GetAttribute(this IHasAttributes element, string attributeName)
        {
            element.LogAction($"Getting attribute value for '{attributeName}'");
            element.Process(e => e.GetAttribute(attributeName));
        }
    }
}