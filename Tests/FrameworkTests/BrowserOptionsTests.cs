namespace Tests.FrameworkTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using Tiver.Fowl.Core.Configuration;
    using Tiver.Fowl.WebDriverExtended.Browsers;

    /// <summary>
    /// Browser options must describe the browser, not the place it runs. The remote branch of both
    /// factories used to construct a bare options object, so every configured switch — headless
    /// above all — was silently dropped the moment a grid address was set.
    ///
    /// Assertions run against the W3C capabilities the options serialize to, which is the payload a
    /// grid actually receives. That covers the defect without a browser or a grid, since the bug was
    /// entirely in which options object got built. What it does not prove is that a real grid
    /// honours them; that needs the container run described in plan 08.
    /// </summary>
    [Parallelizable(ParallelScope.All)]
    public class BrowserOptionsTests
    {
        private static readonly Uri GridAddress = new("http://localhost:4444/wd/hub");

        /// <summary>
        /// Reads the browser arguments back out of serialized capabilities. Firefox exposes no
        /// public argument list on its options type — only <c>ChromiumOptions</c> does — so going
        /// through capabilities is the one route that works for both.
        /// </summary>
        private static IReadOnlyCollection<string> ArgumentsIn(
            OpenQA.Selenium.ICapabilities capabilities, string capabilityName)
        {
            if (capabilities.GetCapability(capabilityName) is not IDictionary<string, object> blob
                || !blob.TryGetValue("args", out var args)
                || args is not IEnumerable list)
            {
                return Array.Empty<string>();
            }

            return list.Cast<object>().Select(a => a?.ToString() ?? string.Empty).ToArray();
        }

        private static IReadOnlyCollection<string> ChromeArguments(BrowserConfiguration configuration)
        {
            var capabilities = ChromeBrowserFactory.BuildOptions(configuration).ToCapabilities();
            return ArgumentsIn(capabilities, "goog:chromeOptions");
        }

        private static IReadOnlyCollection<string> FirefoxArguments(BrowserConfiguration configuration)
        {
            var capabilities = FirefoxBrowserFactory.BuildOptions(configuration).ToCapabilities();
            return ArgumentsIn(capabilities, "moz:firefoxOptions");
        }

        /// <summary>Guards the helper itself: an always-empty reader would pass every DoesNotContain.</summary>
        [Test]
        public void TheArgumentReaderSeesArgumentsAtAll()
        {
            CollectionAssert.IsNotEmpty(ChromeArguments(new BrowserConfiguration { Headless = true }));
            CollectionAssert.IsNotEmpty(FirefoxArguments(new BrowserConfiguration { Headless = true }));
        }

        [Test]
        public void Chrome_AppliesHeadless_OnTheRemotePath()
        {
            CollectionAssert.Contains(
                ChromeArguments(new BrowserConfiguration { Headless = true, RemoteAddress = GridAddress }),
                "--headless");
        }

        [Test]
        public void Chrome_AppliesHeadless_OnTheLocalPath()
        {
            CollectionAssert.Contains(
                ChromeArguments(new BrowserConfiguration { Headless = true }),
                "--headless");
        }

        [Test]
        public void Chrome_OmitsHeadless_WhenNotConfigured()
        {
            CollectionAssert.DoesNotContain(
                ChromeArguments(new BrowserConfiguration { RemoteAddress = GridAddress }),
                "--headless");
        }

        [Test]
        public void Chrome_AppliesDockerSwitches_OnTheRemotePath_WhenConfigured()
        {
            var arguments = ChromeArguments(
                new BrowserConfiguration { RunningInDocker = true, RemoteAddress = GridAddress });

            CollectionAssert.Contains(arguments, "--no-sandbox");
            CollectionAssert.Contains(arguments, "--disable-dev-shm-usage");
        }

        /// <summary>
        /// The <c>/.dockerenv</c> probe describes the machine running the tests, which on a grid is
        /// not the machine running the browser. It must therefore not leak into a remote session —
        /// otherwise a containerised CI agent would silently rewrite the node's configuration.
        /// A grid node that needs the switches is told so via <c>RunningInDocker</c>.
        /// </summary>
        [Test]
        public void Chrome_DoesNotInferDockerFromTheLocalMachine_OnTheRemotePath()
        {
            var arguments = ChromeArguments(new BrowserConfiguration { RemoteAddress = GridAddress });

            CollectionAssert.DoesNotContain(arguments, "--no-sandbox");
            CollectionAssert.DoesNotContain(arguments, "--disable-dev-shm-usage");
        }

        [Test]
        public void Firefox_AppliesHeadless_OnTheRemotePath()
        {
            CollectionAssert.Contains(
                FirefoxArguments(new BrowserConfiguration { Headless = true, RemoteAddress = GridAddress }),
                "-headless");
        }

        [Test]
        public void Firefox_AppliesHeadless_OnTheLocalPath()
        {
            CollectionAssert.Contains(
                FirefoxArguments(new BrowserConfiguration { Headless = true }),
                "-headless");
        }

        [Test]
        public void Firefox_OmitsHeadless_WhenNotConfigured()
        {
            CollectionAssert.DoesNotContain(
                FirefoxArguments(new BrowserConfiguration { RemoteAddress = GridAddress }),
                "-headless");
        }
    }
}
