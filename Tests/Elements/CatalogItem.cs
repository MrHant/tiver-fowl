namespace Tests.Elements;

using OpenQA.Selenium;
using Tiver.Fowl.Logging;
using Tiver.Fowl.ViewBase;

public class CatalogItem : Element
{
    private static readonly Element Card = new Element("//div[contains(@class,'card-block')]/h4[contains(.,'{0}')]/../..");
    
    public CatalogItem(string itemName) : base(Card, itemName, itemName)
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