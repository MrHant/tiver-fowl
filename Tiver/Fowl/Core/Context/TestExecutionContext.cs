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
            get
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
            get
            {
                var value = Context.Test.Read("Browser");
                if (value.GetType() == typeof (NullContextItem))
                {
                    return null;
                }

                return (IBrowser) value;
            }

            set
            {
                Context.Test.Write("Browser", value);
            }
        }

        public static bool IsWebDriverTest
        {
            get
            {
                if (TestType == typeof(NullContextItem))
                {
                    // Initialize Test.TestType before trying to get IsWebDriverTest property.
                    return false;
                }

                var attribute = Attribute.GetCustomAttribute(TestType, typeof(WebDriverTestAttribute));
                return attribute != null;
            }
        }

        public static TestContext TestContext
        {
            get
            {
                return (TestContext)Context.Test.Read("TestContext");
            }

            set
            {
                Context.Test.Write("TestContext", value);
            }
        }
    }
}
