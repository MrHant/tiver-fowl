namespace Tests.Tests
{
    using Elements;
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

        [Test]
        public void ElementLocatorFormatting()
        {
            var el = new Element("//p[text()='{0}']");
            var derivedElement = new Element(el, locatorFormattingArguments: "Hello");
            Assert.IsTrue(derivedElement.LocatorFormattingArguments.Length == 1);
            Assert.IsTrue((string)derivedElement.LocatorFormattingArguments[0] == "Hello");
            Assert.IsTrue(el.LocatorFormattingArguments.Length == 0);

            var derivedButton = new Button(el, locatorFormattingArguments: "Hello");
            Assert.IsTrue(derivedButton.LocatorFormattingArguments.Length == 1);
            Assert.IsTrue((string)derivedButton.LocatorFormattingArguments[0] == "Hello");
        }
    }
}