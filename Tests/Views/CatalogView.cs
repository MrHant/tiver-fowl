namespace Tests.Views
{
    using System.Collections.Generic;
    using System.Linq;
    using Elements;
    using OpenQA.Selenium;
    using Tiver.Fowl.Core.Context;
    using Tiver.Fowl.Waiting;

    public static class CatalogView
    {
        public static readonly Button CategoryMenuItem = new Button("//div[contains(@class,'list-group')]/a[contains(text(),'{0}')]");
        public static readonly Button PhonesMenuItem = new Button(CategoryMenuItem, locatorFormattingArguments: "Phones");
        public static readonly Button LaptopsMenuItem = new Button(CategoryMenuItem, locatorFormattingArguments: "Laptops");
        public static readonly Button MonitorsMenuItem = new Button(CategoryMenuItem, locatorFormattingArguments: "Monitors");
        
        public static IEnumerable<CatalogItem> GetAllItems(int expectedCount = -1)
        {
            var getItemNames = () => TestExecutionContext.WebElementActions.FindSeveral(CatalogItem.ItemLocator)
                .Select(e => e.FindElement(By.XPath("./h4")).Text);
            
            var itemNames = Wait.Until(() =>
            {
                if (expectedCount != -1)
                {
                    var names = getItemNames().ToList();
                    return names.Count != expectedCount ? null : names;
                }
                else
                {
                    return getItemNames();
                }
            });
            return itemNames.Select(name => new CatalogItem(name));
        }
    }
}