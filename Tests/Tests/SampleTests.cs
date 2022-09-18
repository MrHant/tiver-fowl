namespace Tests.Tests
{
    using Views;
    using NUnit.Framework;
    using Tiver.Fowl.Core.Attributes;
    using Tiver.Fowl.TestingBase;
    using Tiver.Fowl.ViewBase;
    using Tiver.Fowl.ViewBase.Behaviors.Extensions;

    [WebDriverTest]
    public class SampleTests : BaseTestForNUnit
    {
        [Test]
        public void SelectCategory()
        {
            this.LogStep("Open 'Laptops' catalog section");
            CatalogView.LaptopsMenuItem.Click();

            this.LogStep("Specific item from catalog is displayed");
            Assert.IsTrue(new Element("//div[contains(@class,'card-block')]/h4[contains(.,'MacBook air')]/a").Displayed());
        }
    }
}