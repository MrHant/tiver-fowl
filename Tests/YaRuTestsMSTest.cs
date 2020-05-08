using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]
namespace Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Tiver.Fowl.Core.Attributes;
    using Tiver.Fowl.TestingBase;
    using Tiver.Fowl.ViewBase;
    using Tiver.Fowl.ViewBase.Behaviors.Extensions;

    [WebDriverTest]
    [TestClass]
    public class YaRuTestsMSTest : BaseTestForMsTest
    {
        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public void Test1(int testCase)
        {
            this.LogStep("Search for something");
            HomePage.SearchBox.Type("example.com");
            HomePage.SearchButton.Click();

            this.LogStep("Results are displayed");
            Assert.IsTrue(new Element("//div[contains(@class,'serp-list')]/div[@class='serp-adv__found']").Displayed());
        }
        
        [TestMethod]
        public void Test2()
        {
            this.LogStep("Search for something");
            HomePage.SearchBox.Type("example.com");
            HomePage.SearchButton.Click();

            this.LogStep("Results are displayed");
            Assert.IsTrue(new Element("//div[contains(@class,'serp-list')]/div[@class='serp-adv__found']").Displayed());
        }
        
        [TestMethod]
        public void Test3()
        {
            this.LogStep("Search for something");
            HomePage.SearchBox.Type("example.com");
            HomePage.SearchButton.Click();

            this.LogStep("Results are displayed");
            Assert.IsTrue(new Element("//div[contains(@class,'serp-list')]/div[@class='serp-adv__found']").Displayed());
        }
    }
}