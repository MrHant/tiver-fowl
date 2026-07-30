namespace Tiver.Fowl.Core.Context
{
    using System;
    using Attributes;
    using Enums;
    using WebDriverExtended.Browsers;

    public static class TestExecutionContext
    {
        public static string SessionId
        {
            get => Context.Session.Read<string>("SessionId");
            set => Context.Session.Write("SessionId", value);
        }

        public static Type TestType
        {
            private get => Context.Test.Read<Type>("TestType");
            set => Context.Test.Write("TestType", value);
        }

        public static IBrowser Browser
        {
            private get => Context.Test.Read<IBrowser>("Browser");
            set => Context.Test.Write("Browser", value);
        }

        public static string TestName
        {
            get => Context.Test.Read<string>("TestName");
            set => Context.Test.Write("TestName", value);
        }

        /// <summary>
        /// The current test name, or <c>null</c> when no test scope is active or the name has not
        /// been set yet. For ambient consumers such as log enrichers, which also run during session
        /// setup, report generation and teardown.
        /// </summary>
        public static string CurrentTestNameOrNull =>
            Context.TestOrNull is { } storage && storage.TryRead<string>("TestName", out var name)
                ? name
                : null;

        public static TestResult TestResult
        {
            get => Context.Test.Read<TestResult>("TestResult");
            set => Context.Test.Write("TestResult", value);
        }

        public static int TestStep
        {
            get => Context.Test.ReadOrInit("TestStep", 0);
            set => Context.Test.Write("TestStep", value);
        }

        public static DateTime TestStartTime
        {
            get => Context.Test.ReadOrInit("TestStartTime", DateTime.MinValue);
            set => Context.Test.Write("TestStartTime", value);
        }

        public static IBrowserActions BrowserActions => Browser.BrowserActions;

        public static IWebElementActions WebElementActions => Browser.WebElementActions;

        public static bool IsWebDriverTest
        {
            get
            {
                var attribute = Attribute.GetCustomAttribute(TestType, typeof(WebDriverTestAttribute));
                return attribute != null;
            }
        }
    }
}
