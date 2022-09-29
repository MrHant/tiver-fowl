namespace Tests.Elements;

using OpenQA.Selenium;
using Tiver.Fowl.Logging;
using Tiver.Fowl.ViewBase;

public class CatalogItem : Element
{
    public const string ItemLocator = "//div[contains(@class,'card-block')]";
    private static readonly Element Item = new Element($"{ItemLocator}/h4[contains(.,'{{0}}')]/../..");
    
    public CatalogItem(string itemName) : base(Item, itemName, itemName)
    {
    }

    public string GetPrice()
    {
        this.LogAction("Get price");
        return Process(e =>
            e.FindElement(By.XPath("./div/h5")).Text);
    }
    
    public string GetDescription()
    {
        this.LogAction("Get description");
        return Process(e =>
            e.FindElement(By.XPath("./div/p")).Text);
    }
}