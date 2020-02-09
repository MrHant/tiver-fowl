namespace Tests.Elements
{
    using Tiver.Fowl.ViewBase;
    using Tiver.Fowl.ViewBase.Behaviors;

    public class Button: Element, IClickable
    {
        public Button(string locator, string name = null) : base(locator, name)
        {
        }
    }
}