namespace Tiver.Fowl.ViewBase
{
    using OpenQA.Selenium;

    public class Textbox : Element
    {
        public Textbox(string locator)
            : base(locator)
        {
        }

        public void Type(string value)
        {
            this.Process(e => e.SendKeys(value));
        }

        public void PressEnter()
        {
            this.Process(e => e.SendKeys(Keys.Enter));
        }
    }
}
