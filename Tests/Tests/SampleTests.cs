namespace Tests.Tests
{
    using System.Linq;
    using Tiver.Fowl.Core.Configuration;
    using Elements;
    using Views;
    using NUnit.Framework;
    using Tiver.Fowl.Core.Attributes;
    using Tiver.Fowl.TestingBase;
    using Tiver.Fowl.ViewBase.Behaviors.Extensions;
    using NUnit.Framework.Legacy;

    [WebDriverTest]
    public class SampleTests : BaseTestForNUnit
    {
        [Test]
        public void SelectCategory()
        {
            ActiveConfiguration.NavigateTo("home");

            this.LogStep("Open 'Laptops' catalog section");
            CatalogView.LaptopsMenuItem.Click();
            
            this.LogStep("Specific item from catalog is displayed");
            var macbook = new CatalogItem("MacBook air");
            ClassicAssert.IsTrue(macbook.Displayed());
            ClassicAssert.AreEqual("$700", macbook.GetPrice());
            ClassicAssert.AreEqual(
                "1.6GHz dual-core Intel Core i5 (Turbo Boost up to 2.7GHz) with 3MB shared L3 cache Configurable"
                + " to 2.2GHz dual-core Intel Core i7 (Turbo Boost up to 3.2GHz) with 4MB shared L3 cache.",
                macbook.GetDescription());
        }
        
        [Test]
        public void CheckNumberOfItems()
        {
            ActiveConfiguration.NavigateTo("home");

            this.LogStep("Open 'Monitors' catalog section");
            CatalogView.MonitorsMenuItem.Click();
            
            this.LogStep("Check that two monitors are displayed");
            var monitors = CatalogView.GetAllItems(2).ToList();
            ClassicAssert.AreEqual(2, monitors.Count());
            ClassicAssert.IsTrue(monitors.All(m => !string.IsNullOrEmpty(m.GetPrice()) && !string.IsNullOrEmpty(m.GetDescription())));
        }
    }
}