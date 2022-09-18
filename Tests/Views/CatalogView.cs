namespace Tests.Views
{
    using Elements;

    public static class CatalogView
    {
        public static readonly Button CategoryMenuItem = new Button("//div[contains(@class,'list-group')]/a[contains(text(),'{0}')]");
        public static readonly Button PhonesMenuItem = new Button(CategoryMenuItem, locatorFormattingArguments: "Phones");
        public static readonly Button LaptopsMenuItem = new Button(CategoryMenuItem, locatorFormattingArguments: "Laptops");
        public static readonly Button MonitorsMenuItem = new Button(CategoryMenuItem, locatorFormattingArguments: "Monitors");
    }
}