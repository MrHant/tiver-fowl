namespace Tests
{
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
            this.LogStep("Open 'Quick View' for first displaying item");
            NavigationView.Dresses.Click();

            this.LogStep("Count of items displayed");
            Assert.IsTrue(new Element("//h1[contains(@class, 'page-heading')]/span[@class='heading-counter']").Displayed());
        }
    }
}