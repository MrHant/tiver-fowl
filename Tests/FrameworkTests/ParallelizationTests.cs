namespace Tests.FrameworkTests;

using System.Collections.Generic;
using Tiver.Fowl.Core.Configuration;
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
    [TestCaseSource(nameof(GetCatalogTestData))]
    public void SelectCatalogSection((string menuItemText, string expectedItemName) testData)
    {
        ActiveConfiguration.NavigateTo("home");

        this.LogStep("Open catalog section");
        CatalogView.CategoryMenuItem.Click(testData.menuItemText);

        this.LogStep("Specific item from catalog is displayed");
        ClassicAssert.IsTrue(new Element("//div[contains(@class,'card-block')]/h4[contains(.,'{0}')]/a").Displayed(testData.expectedItemName));
    }

    private static IEnumerable<(string menuItemText, string expectedItemName)> GetCatalogTestData()
    {
        yield return (ActiveConfiguration.Get<string>("TestData:CategoryName")!, "MacBook air");
        yield return ("Phones", "Nexus 6");
        yield return ("Monitors", "ASUS Full HD");
    }
}