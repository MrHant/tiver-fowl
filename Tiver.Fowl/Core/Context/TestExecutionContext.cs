namespace Tiver.Fowl.Core.Context
{
    using System;
    using Attributes;
    using Enums;
    using WebDriverExtended.Browsers;

    public static class TestExecutionContext
    {
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
