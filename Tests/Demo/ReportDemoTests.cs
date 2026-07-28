namespace Tests.Demo
{
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using Tiver.Fowl.Core.Attributes;
    using Tiver.Fowl.Core.Configuration;
    using Tiver.Fowl.TestingBase;
    using Tiver.Fowl.ViewBase;
    using Tiver.Fowl.ViewBase.Behaviors.Extensions;
    using Views;

    /// <summary>
    /// Browser-driven demo fixture. Produces steps, element actions, a passing test and a
    /// deliberate failure (with screenshot) for the sample HTML report.
    /// </summary>
    [Explicit("Demo data for the HTML report - run via 'task report-demo'")]
    [WebDriverTest]
    public class CatalogDemoTests : BaseTestForNUnit
    {
        [Test]
        public void BrowseCatalogCategory()
        {
            ActiveConfiguration.NavigateTo("home");

            this.LogStep("Open the Laptops category");
            CatalogView.LaptopsMenuItem.Click();

            this.LogStep("Catalog lists the expected laptop");
            ClassicAssert.IsTrue(
                new Element("//div[contains(@class,'card-block')]/h4[contains(.,'{0}')]/a").Displayed("MacBook air"));
        }

        [Test]
        public void PromotionalBannerIsDisplayed()
        {
            ActiveConfiguration.NavigateTo("home");

            this.LogStep("Open the Phones category");
            CatalogView.PhonesMenuItem.Click();

            // Fails on purpose so the report has a failed test to render (with screenshot)
            this.LogStep("Verify the promotional banner");
            ClassicAssert.Fail("Expected the promotional banner to be displayed, but the catalog rendered without it.");
        }
    }

    /// <summary>
    /// Browser-free demo fixture. Lands in its own report group and contributes a skipped test.
    /// </summary>
    [Explicit("Demo data for the HTML report - run via 'task report-demo'")]
    public class SmokeDemoTests : BaseTestForNUnit
    {
        [Test]
        public void ConfigurationIsLoaded()
        {
            this.LogStep("Read the configured category from config.json");
            ClassicAssert.AreEqual("Laptops", ActiveConfiguration.Get<string>("TestData:CategoryName"));

            this.LogStep("Named URLs resolve");
            ClassicAssert.IsNotNull(ActiveConfiguration.GetUrl("home"));
        }

        [Test]
        public void PendingFeature()
        {
            this.LogStep("Feature is not implemented yet");
            Assert.Ignore("Skipped on purpose so the report has a skipped test to render.");
        }
    }
}
