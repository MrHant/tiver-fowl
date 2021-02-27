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
            private get
            {
                return (Type)Context.Test.Read("TestType");
            }

            set
            {
                Context.Test.Write("TestType", value);
            }
        }

        public static IBrowser Browser
        {
            private get
            {
                return (IBrowser) Context.Test.Read("Browser");
            }

            set
            {
                Context.Test.Write("Browser", value);
            }
        }

        public static string TestName
        {
            get
            {
                return (string)Context.Test.Read("TestName");
            }

            set
            {
                Context.Test.Write("TestName", value);
            }
        }

        public static TestResult TestResult
        {
            get
            {
                return (TestResult)Context.Test.Read("TestResult");
            }

            set
            {
                Context.Test.Write("TestResult", value);
            }
        }

        public static int TestStep
        {
            get
            {
                return (int)Context.Test.ReadOrAdd("TestStep", 0);
            }

            set
            {
                Context.Test.Write("TestStep", value);
            }
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
