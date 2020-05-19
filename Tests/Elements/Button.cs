namespace Tests.Elements
{
    using System.Runtime.CompilerServices;
    using Tiver.Fowl.ViewBase;
    using Tiver.Fowl.ViewBase.Behaviors;

    public class Button: Element, IClickable
    {
        public Button(string locator, [CallerMemberName]string name = null) 
            : base(locator, $"{nameof(Button)} {name}")
        {
        }
    }
}