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
                var value = Context.Test.Read("TestType");
                if (value.GetType() == typeof(NullContextItem))
                {
                    return null;
                }

                return (Type)value;
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
                if (TestType == null)
                {
                    // Initialize Test.TestType before trying to get IsWebDriverTest property.
                    return false;
                }

                var attribute = Attribute.GetCustomAttribute(TestType, typeof(WebDriverTestAttribute));
                return attribute != null;
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
    }
}
