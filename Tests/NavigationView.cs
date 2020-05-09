namespace Tests
{
    using Elements;

    public static class NavigationView
    {
        public static readonly Button Dresses = new Button("//div[@id='block_top_menu']/ul/li/a[text()='Dresses']", "'Dresses' menu item");
    }
}