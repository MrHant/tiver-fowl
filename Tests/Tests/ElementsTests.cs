namespace Tests.Tests
{
    using Views;
    using NUnit.Framework;
    using Tiver.Fowl.Core.Attributes;
    using Tiver.Fowl.ViewBase;
    using Tiver.Fowl.ViewBase.Behaviors.Extensions;
    using Tiver.Fowl.ViewBase.Exceptions;
    using Tiver.Fowl.Waiting.Exceptions;

    [WebDriverTest]
    public class ElementsTests : BaseTestForNUnit
    {
        [Test]
        public void BasicElementsMethods()
        {
            using (new Frame("//iframe[@id='iframe']"))
            {
                // Element
                Assert.IsNotEmpty(NavigationView.TopMenuItem.Locator);
                Assert.IsNotEmpty(NavigationView.TopMenuItem.Name);

                // IHasAttributes
                Assert.IsNotNull(NavigationView.TopMenuItem.GetAttribute("title", "Women"));
                Assert.IsNull(NavigationView.TopMenuItem.GetAttribute("xxxxx", "Women"));

                // IVisible
                Assert.IsTrue(NavigationView.TopMenuItem.Displayed("Women"));
                Assert.Throws<WaitTimeoutException>(() =>
                {
                    Assert.IsFalse(NavigationView.TopMenuItem.Displayed("xxxxx"));
                });
            }
        }

        [Test]
        public void InvalidLocatorFormatting()
        {
            Assert.Throws<LocatorFormattingException>(() =>
            {
                NavigationView.TopMenuItem.GetAttribute("title");
            });
        }
    }
}