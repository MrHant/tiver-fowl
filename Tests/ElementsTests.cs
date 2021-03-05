namespace Tests
{
    using NUnit.Framework;
    using Tiver.Fowl.Core.Attributes;
    using Tiver.Fowl.ViewBase.Behaviors.Extensions;
    using Tiver.Fowl.ViewBase.Exceptions;

    [WebDriverTest]
    public class ElementsTests : BaseTestForNUnit
    {
        [Test]
        public void BasicElementsMethods()
        {
            // Element
            Assert.IsNotEmpty(NavigationView.TopMenuItem.Locator);
            Assert.IsNotEmpty(NavigationView.TopMenuItem.Name);
            
            // IHasAttributes
            Assert.IsNotNull(NavigationView.TopMenuItem.GetAttribute("title", "Dresses"));
            Assert.IsNull(NavigationView.TopMenuItem.GetAttribute("xxxxx", "Dresses"));
            
            // IVisible
            Assert.IsTrue(NavigationView.TopMenuItem.Displayed("Dresses"));
            Assert.IsFalse(NavigationView.TopMenuItem.Displayed("xxxxx"));
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