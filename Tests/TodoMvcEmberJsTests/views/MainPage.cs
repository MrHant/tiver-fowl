namespace TodoMvcEmberJsTests.views
{
    using Tiver.Fowl.ViewBase;

    public class MainPage : View
    {
        public static readonly Textbox NewTaskField = new Textbox("//input[@id='new-todo']", "New task field");
    }
}
