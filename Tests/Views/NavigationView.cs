namespace Tests.Views
{
    using Elements;

    public static class NavigationView
    {
        public static readonly Button TopMenuItem = new Button("//div[@id='menu-primary']/nav/ul/li/a[contains(text(), '{0}')]");
    }
}