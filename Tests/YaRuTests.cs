namespace Tests
{
    using NUnit.Framework;
    using Tiver.Fowl.Core.Attributes;
    using Tiver.Fowl.ViewBase;

    [WebDriverTest]
    public class YaRuTests : BaseTestForNUnit
    {
        [Test]
        public void Test1()
        {
            new Element("//input[@id='text']").Process(e => e.SendKeys("example.com"));
            new Element("//button[@type='submit']").Process(e => e.Click());
            Assert.IsTrue(new Element("//div[.='Example Domain']").Process(e => e.Displayed));
            
        }
    }
}