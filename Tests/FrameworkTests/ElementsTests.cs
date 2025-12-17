namespace Tests.FrameworkTests
{
    using Configuration;
    using Elements;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using Tiver.Fowl.Core.Attributes;
    using Tiver.Fowl.ViewBase;
    using Tiver.Fowl.ViewBase.Behaviors.Extensions;
    using Tiver.Fowl.ViewBase.Exceptions;
    using Tiver.Fowl.Waiting.Exceptions;
    using Views;

    [WebDriverTest]
    public class ElementsTests : BaseTestForNUnit
    {
        [Test]
        public void BasicElementsMethods()
        {
            ActiveConfiguration.NavigateTo("home");

            // Element
            ClassicAssert.IsNotEmpty(CatalogView.CategoryMenuItem.Locator);
            ClassicAssert.IsNotEmpty(CatalogView.CategoryMenuItem.Name);

            // IHasAttributes
            ClassicAssert.IsNotNull(CatalogView.CategoryMenuItem.GetAttribute("title", "Laptops"));
            ClassicAssert.IsNull(CatalogView.CategoryMenuItem.GetAttribute("xxxxx", "Laptops"));

            // IVisible
            ClassicAssert.IsTrue(CatalogView.CategoryMenuItem.Displayed("Laptops"));
            Assert.Throws<WaitTimeoutException>(() =>
            {
                ClassicAssert.IsFalse(CatalogView.CategoryMenuItem.Displayed("xxxxx"));
            });
        }

        [Test]
        public void InvalidLocatorFormatting()
        {
            ActiveConfiguration.NavigateTo("home");

            Assert.Throws<LocatorFormattingException>(() =>
            {
                CatalogView.CategoryMenuItem.GetAttribute("title");
            });
        }

        [Test]
        public void ElementLocatorFormatting()
        {
            ActiveConfiguration.NavigateTo("home");

            var el = new Element("//p[text()='{0}']");
            var derivedElement = new Element(el, locatorFormattingArguments: "Hello");
            ClassicAssert.IsTrue(derivedElement.LocatorFormattingArguments.Length == 1);
            ClassicAssert.IsTrue((string)derivedElement.LocatorFormattingArguments[0] == "Hello");
            ClassicAssert.IsTrue(el.LocatorFormattingArguments.Length == 0);

            var derivedButton = new Button(el, locatorFormattingArguments: "Hello");
            ClassicAssert.IsTrue(derivedButton.LocatorFormattingArguments.Length == 1);
            ClassicAssert.IsTrue((string)derivedButton.LocatorFormattingArguments[0] == "Hello");
        }
    }
}