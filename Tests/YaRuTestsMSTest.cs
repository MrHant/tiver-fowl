﻿namespace Tests
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
        [DataRow(3)]
        public void Test1(int parallel)
        {
            this.LogStep("Search for something");
            HomePage.SearchBox.Type("example.com");
            HomePage.SearchButton.Click();

            this.LogStep("Results are displayed");
            Assert.IsTrue(new Element("//div[.='Example Domain']").Displayed());
        }
    }
}