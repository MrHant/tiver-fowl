namespace Tests
{
    using Elements;

    public static class NavigationView
    {
        public static readonly Button TopMenuItem = new Button("//div[@id='block_top_menu']/ul/li/a[text()='{0}']");
    }
}