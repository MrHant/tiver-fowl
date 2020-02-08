namespace Tests
{
    using NUnit.Framework;
    using Tiver.Fowl.Core.Attributes;
    using Tiver.Fowl.TestingBase;
    using Tiver.Fowl.ViewBase;
    using Tiver.Fowl.ViewBase.Behaviors.Extensions;
    
    [WebDriverTest]
    public class YaRuTests : BaseTestForNUnit
    {
        [Test]
        public void Test1()
        {
            this.LogStep("Search for something");
            HomePage.SearchBox.Type("example.com");
            HomePage.SearchButton.Click();

            this.LogStep("Results are displayed");
            Assert.IsTrue(new Element("//div[.='Example Domain']").Displayed());
        }
    }
}