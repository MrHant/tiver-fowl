namespace Tests.FrameworkTests
{
    using Tiver.Fowl.Core.Configuration;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using Tiver.Fowl.Core.Attributes;
    using Tiver.Fowl.ViewBase.Behaviors.Extensions;
    using Views;

    [WebDriverTest]
    public class ConfigurationTests : BaseTestForNUnit
    {
        [Test]
        public void ConfigurationEnvironmentOverride()
        {
            // Verify environment is null by default
            ClassicAssert.AreEqual(null, ActiveConfiguration.Environment);

            // Verify config value comes from config.json file
            var categoryName = ActiveConfiguration.Get<string>("TestData:CategoryName")!;
            ClassicAssert.AreEqual("Laptops", categoryName);

            // Override environment to "qa" 
            ActiveConfiguration.SetEnvironment("qa");

            // Verify environment was set
            ClassicAssert.AreEqual("qa", ActiveConfiguration.Environment);

            // Verify config value comes from qa override
            categoryName = ActiveConfiguration.Get<string>("TestData:CategoryName")!;
            ClassicAssert.AreEqual("Phones", categoryName);

            // Verify it works with actual page interaction
            ActiveConfiguration.NavigateTo("home");
            ClassicAssert.IsTrue(CatalogView.CategoryMenuItem.Displayed(categoryName));
        }
    }
}
