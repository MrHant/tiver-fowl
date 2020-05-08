namespace Tests
{
    using NUnit.Framework;
    using Tiver.Fowl.Core.Attributes;
    using Tiver.Fowl.TestingBase;
    using Tiver.Fowl.ViewBase;
    using Tiver.Fowl.ViewBase.Behaviors.Extensions;
    
    [WebDriverTest]
    [Parallelizable(ParallelScope.All)]
    public class YaRuTests : BaseTestForNUnit
    {
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Test1(int parallel)
        {
            this.LogStep("Search for something");
            HomePage.SearchBox.Type("example.com");
            HomePage.SearchButton.Click();

            this.LogStep("Results are displayed");
            Assert.IsTrue(new Element("//div[contains(@class,'serp-list')]/div[@class='serp-adv__found']").Displayed());
        }
    }
}