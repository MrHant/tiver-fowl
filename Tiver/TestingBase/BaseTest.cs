namespace Tiver.TestingBase
{
    using Tiver.Core.Context;

    public abstract class BaseTest
    {
        internal BaseTest()
        {
            TestExecutionContext.TestType = GetType();
        }
    }
}
