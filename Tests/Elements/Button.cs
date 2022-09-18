namespace Tests.Elements
{
    using System.Runtime.CompilerServices;
    using Tiver.Fowl.ViewBase;
    using Tiver.Fowl.ViewBase.Behaviors;

    public class Button: Element, IClickable
    {
        public Button(string locator, [CallerMemberName]string name = null) : base(locator, name)
        {
        }

        public Button(Element source, [CallerMemberName]string name = null, params object[] locatorFormattingArguments) : base(source, name, locatorFormattingArguments)
        {
        }
    }
}