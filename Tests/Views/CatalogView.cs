namespace Tests.Views
{
    using Elements;

    public static class CatalogView
    {
        public static readonly Button CategoryMenuItem = new Button("//div[contains(@class,'list-group')]/a[contains(text(),'{0}')]");
    }
}