namespace Tests.FrameworkTests
{
    using System;
    using System.Drawing;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using Tiver.Fowl.Core.Configuration;
    using Tiver.Fowl.WebDriverExtended.Browsers;
    using Tiver.Fowl.WebDriverExtended.Exceptions;

    /// <summary>
    /// A resolution is either fully specified or not specified at all. Setting one dimension used to
    /// convert the other to zero and hand the driver a degenerate window, which then failed every
    /// interaction with errors pointing nowhere near the configuration file.
    /// </summary>
    [Parallelizable(ParallelScope.All)]
    public class WindowSizeTests
    {
        [Test]
        public void BothDimensions_ResolveToThatSize()
        {
            var size = BrowserFactory.ResolveWindowSize(new Resolution { Width = 1200, Height = 800 });

            ClassicAssert.AreEqual(new Size(1200, 800), size);
        }

        [Test]
        public void NoResolution_LeavesTheBrowserAtItsDefault()
        {
            ClassicAssert.IsNull(BrowserFactory.ResolveWindowSize(null));
        }

        [Test]
        public void EmptyResolution_LeavesTheBrowserAtItsDefault()
        {
            ClassicAssert.IsNull(BrowserFactory.ResolveWindowSize(new Resolution()));
        }

        [Test]
        public void WidthWithoutHeight_ReportsWhichDimensionIsMissing()
        {
            var exception = Assert.Throws<IncorrectBrowserConfigurationException>(
                () => BrowserFactory.ResolveWindowSize(new Resolution { Width = 1200 }));

            StringAssert.Contains("Height", exception!.Message);
        }

        [Test]
        public void HeightWithoutWidth_ReportsWhichDimensionIsMissing()
        {
            var exception = Assert.Throws<IncorrectBrowserConfigurationException>(
                () => BrowserFactory.ResolveWindowSize(new Resolution { Height = 800 }));

            StringAssert.Contains("Width", exception!.Message);
        }

        /// <summary>
        /// The same degenerate window, reached by configuring the zero rather than omitting the
        /// dimension. Binding turns <c>"Width": 0</c> into a real 0, not a null.
        /// </summary>
        [TestCase(0, 800)]
        [TestCase(1200, 0)]
        [TestCase(-1, 800)]
        public void NonPositiveDimensions_AreRejected(int width, int height)
        {
            Assert.Throws<IncorrectBrowserConfigurationException>(
                () => BrowserFactory.ResolveWindowSize(new Resolution { Width = width, Height = height }));
        }

        /// <summary>
        /// The configuration is validated before the driver is created, so a bad resolution cannot
        /// leave an orphaned browser process behind. Pointing at a closed port proves the ordering:
        /// if the driver were built first, this would surface as a connection failure instead.
        /// </summary>
        [Test]
        public void InvalidResolution_IsReportedBeforeAnyBrowserIsLaunched()
        {
            var configuration = new BrowserConfiguration
            {
                BrowserType = "chrome",
                RemoteAddress = new Uri("http://127.0.0.1:1/"),
                Resolution = new Resolution { Width = 1200 },
            };

            Assert.Throws<IncorrectBrowserConfigurationException>(
                () => new ChromeBrowserFactory().Build(configuration));
        }
    }
}
