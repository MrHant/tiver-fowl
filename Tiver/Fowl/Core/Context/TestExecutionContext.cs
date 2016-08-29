namespace Tiver.Fowl.Core.Context
{
    using System;
    using Attributes;
    using WebDriverExtended.Contracts.Browsers;

    public static class TestExecutionContext
    {
        public static Type TestType
        {
            get
            {
                return (Type)Context.Test.Get("TestType");
            }

            set
            {
                Context.Test.Add("TestType", value);
            }
        }

        public static IBrowser Browser
        {
            get
            {
                return (IBrowser)Context.Test.Get("Browser");
            }

            set
            {
                Context.Test.Add("Browser", value);
            }
        }

        public static bool IsWebDriverTest
        {
            get
            {
                if (TestExecutionContext.TestType.Equals(typeof(NullContextItem)))
                {
                    // Initialize Test.TestType before trying to get IsWebDriverTest property.
                    return false;
                }

                var attribute = Attribute.GetCustomAttribute(TestExecutionContext.TestType, typeof(WebDriverTestAttribute));
                return attribute != null;
            }
        }
    }
}
