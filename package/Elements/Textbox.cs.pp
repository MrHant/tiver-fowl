namespace $rootnamespace$.Elements
{
    using Behaviors;

    public class Textbox : Element, ITypeable
    {
        public Textbox(string locator) : base(locator)
        {
        }

        public Textbox(string locator, string name) : base(locator, name)
        {
        }
    }
}
