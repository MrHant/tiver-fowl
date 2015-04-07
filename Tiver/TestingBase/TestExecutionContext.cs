namespace Tiver.TestingBase
{
    using System;
    using Tiver.TestingBase.Attributes;
    using Tiver.WebDriverExtended.Browsers;

    public class TestExecutionContext
    {
        public TestExecutionContext(Type testType)
        {
            this.TestType = testType;
        }

        public Type TestType { get; private set; }

        public Browser Browser { get; set; }

        public bool IsWebDriverTest
        {
            get
            {
                var attribute = Attribute.GetCustomAttribute(this.TestType, typeof(WebDriverTestAttribute));
                return attribute != null;
            }
        }
    }
}
