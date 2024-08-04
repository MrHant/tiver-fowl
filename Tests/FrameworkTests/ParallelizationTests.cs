namespace Tests.FrameworkTests;

using NUnit.Framework;
using NUnit.Framework.Legacy;
using Tiver.Fowl.Core.Attributes;
using Tiver.Fowl.TestingBase;
using Tiver.Fowl.ViewBase;
using Tiver.Fowl.ViewBase.Behaviors.Extensions;
using Views;

[WebDriverTest]
[Parallelizable(ParallelScope.All)]
public class ParallelizationTests : BaseTestForNUnit
{
    [Test]
    [TestCase("Laptops", "MacBook air")]
    [TestCase("Phones", "Nexus 6")]
    [TestCase("Monitors", "ASUS Full HD")]
    public void SelectCatalogSection(string menuItemText, string expectedItemName)
    {
        this.LogStep("Open catalog section");
        CatalogView.CategoryMenuItem.Click(menuItemText);

        this.LogStep("Specific item from catalog is displayed");
        ClassicAssert.IsTrue(new Element("//div[contains(@class,'card-block')]/h4[contains(.,'{0}')]/a").Displayed(expectedItemName));
    }
}