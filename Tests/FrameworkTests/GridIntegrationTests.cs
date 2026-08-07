namespace Tests.FrameworkTests
{
    using System;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using Tiver.Fowl.Core.Configuration;
    using Tiver.Fowl.WebDriverExtended.Browsers;

    /// <summary>
    /// End-to-end proof that a grid session honours the configured options — the half
    /// <see cref="BrowserOptionsTests"/> cannot reach, since it stops at the capabilities.
    ///
    /// <para><b>Excluded from the default run.</b> These need a Selenium Grid on
    /// <c>TIVER_GRID_ADDRESS</c> (default <c>http://localhost:4444/</c>). Start one with:</para>
    /// <code>
    /// docker run -d --name tiver-grid --shm-size=2g -p 4444:4444 selenium/standalone-chrome
    /// </code>
    /// <para>Run them with <c>dotnet test --filter "TestCategory=Grid"</c>. Plan 08 covers wiring
    /// this into CI, where the grid can be a service container.</para>
    /// </summary>
    [Explicit("Requires a running Selenium Grid; see class remarks.")]
    [Category("Grid")]
    public class GridIntegrationTests
    {
        private static Uri GridAddress =>
            new(Environment.GetEnvironmentVariable("TIVER_GRID_ADDRESS") ?? "http://localhost:4444/");

        /// <summary>
        /// Chrome reports headless mode in its user agent, which makes it the one signal available
        /// from inside the session that says what the node actually launched.
        /// </summary>
        private static string UserAgentFor(BrowserConfiguration configuration)
        {
            using var browser = new ChromeBrowserFactory().Build(configuration);
            return browser.BrowserActions.ExecuteScript("return navigator.userAgent;")?.ToString()
                   ?? string.Empty;
        }

        /// <summary>
        /// The regression this plan exists for: before the fix, the remote branch built a bare
        /// options object, so a grid session came up headed no matter what the configuration said.
        /// </summary>
        [Test]
        public void ConfiguredHeadless_ReachesTheGridSession()
        {
            var userAgent = UserAgentFor(new BrowserConfiguration
            {
                BrowserType = "chrome",
                Headless = true,
                RemoteAddress = GridAddress,
            });

            StringAssert.Contains("HeadlessChrome", userAgent);
        }

        /// <summary>
        /// The control. Without it, a node that happened to be headless by default would make the
        /// test above pass regardless of whether the option was transmitted.
        /// </summary>
        [Test]
        public void UnconfiguredHeadless_LeavesTheGridSessionHeaded()
        {
            var userAgent = UserAgentFor(new BrowserConfiguration
            {
                BrowserType = "chrome",
                RemoteAddress = GridAddress,
            });

            StringAssert.DoesNotContain("HeadlessChrome", userAgent);
            StringAssert.Contains("Chrome", userAgent);
        }

        [Test]
        public void ConfiguredResolution_ReachesTheGridSession()
        {
            using var browser = new ChromeBrowserFactory().Build(new BrowserConfiguration
            {
                BrowserType = "chrome",
                Headless = true,
                RemoteAddress = GridAddress,
                Resolution = new Resolution { Width = 1024, Height = 768 },
            });

            var width = Convert.ToInt32(browser.BrowserActions.ExecuteScript("return window.outerWidth;"));

            ClassicAssert.AreEqual(1024, width);
        }
    }
}
