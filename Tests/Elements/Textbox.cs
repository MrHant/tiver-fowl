namespace Tests.Elements
{
    using System.Runtime.CompilerServices;
    using Tiver.Fowl.ViewBase;
    using Tiver.Fowl.ViewBase.Behaviors;

    public class Textbox : Element, ITypeable
    {
        public Textbox(string locator, [CallerMemberName]string name = null)
            : base(locator, $"{nameof(Textbox)} {name}")
        {
        }
        
        public Textbox(Element source, [CallerMemberName]string name = null, params object[] locatorFormattingArguments) : base(source, name, locatorFormattingArguments)
        {
        }
    }
}