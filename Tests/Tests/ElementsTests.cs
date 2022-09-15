namespace Tests.Tests
{
    using Views;
    using NUnit.Framework;
    using Tiver.Fowl.Core.Attributes;
    using Tiver.Fowl.ViewBase.Behaviors.Extensions;
    using Tiver.Fowl.ViewBase.Exceptions;
    using Tiver.Fowl.Waiting.Exceptions;

    [WebDriverTest]
    public class ElementsTests : BaseTestForNUnit
    {
        [Test]
        public void BasicElementsMethods()
        {
            // Element
            Assert.IsNotEmpty(CatalogView.CategoryMenuItem.Locator);
            Assert.IsNotEmpty(CatalogView.CategoryMenuItem.Name);

            // IHasAttributes
            Assert.IsNotNull(CatalogView.CategoryMenuItem.GetAttribute("title", "Laptops"));
            Assert.IsNull(CatalogView.CategoryMenuItem.GetAttribute("xxxxx", "Laptops"));

            // IVisible
            Assert.IsTrue(CatalogView.CategoryMenuItem.Displayed("Laptops"));
            Assert.Throws<WaitTimeoutException>(() =>
            {
                Assert.IsFalse(CatalogView.CategoryMenuItem.Displayed("xxxxx"));
            });
        }

        [Test]
        public void InvalidLocatorFormatting()
        {
            Assert.Throws<LocatorFormattingException>(() =>
            {
                CatalogView.CategoryMenuItem.GetAttribute("title");
            });
        }
    }
}