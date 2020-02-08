namespace Tests
{
    using Elements;
    using Tiver.Fowl.ViewBase;

    public class HomePage : View
    {
        public static readonly Textbox SearchBox = new Textbox("//input[@id='text']", "Search Textbox");
        public static readonly Button SearchButton = new Button("//button[@type='submit']", "Search Button");
    }
}