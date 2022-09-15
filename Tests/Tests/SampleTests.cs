namespace Tests.Tests
{
    using Views;
    using NUnit.Framework;
    using Tiver.Fowl.Core.Attributes;
    using Tiver.Fowl.TestingBase;
    using Tiver.Fowl.ViewBase;
    using Tiver.Fowl.ViewBase.Behaviors.Extensions;

    [WebDriverTest]
    [Parallelizable(ParallelScope.All)]
    public class SampleTests : BaseTestForNUnit
    {
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Test1(int parallel)
        {
            this.LogStep("Open 'Laptops' catalog section");
            CatalogView.CategoryMenuItem.Click("Laptops");

            this.LogStep("Specific item from catalog is displayed");
            Assert.IsTrue(new Element("//div[contains(@class,'card-block')]/h4[contains(.,'MacBook air')]/a").Displayed());
        }
    }
}