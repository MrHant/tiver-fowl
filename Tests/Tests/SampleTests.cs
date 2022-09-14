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
            using (new Frame("//iframe[@id='iframe']"))
            {
                this.LogStep("Open 'Women' catalog section");
                NavigationView.TopMenuItem.Click("Women");

                this.LogStep("Count of items displayed");
                Assert.IsTrue(new Element("//p[@class='woocommerce-result-count']").Displayed());
            }
        }
    }
}