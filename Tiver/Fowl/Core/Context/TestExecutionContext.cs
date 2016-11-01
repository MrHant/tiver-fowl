namespace Tiver.Fowl.Core.Context
{
    using System;
    using Attributes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WebDriverExtended.Contracts.Browsers;

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

        public static UnitTestOutcome TestOutcome
        {
            get
            {
                return (UnitTestOutcome)Context.Test.Read("TestOutcome");
            }

            set
            {
                Context.Test.Write("TestOutcome", value);
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
