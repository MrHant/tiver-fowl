namespace Tiver.TestingBase
{
    using Tiver.Core.Context;
    using Tiver.TestingBase.Contracts;

    public abstract class BaseTest : IBaseTest
    {
        internal BaseTest()
        {
            TestExecutionContext.TestType = GetType();
        }

        public abstract void SetUp();

        public abstract void TearDown();
    }
}
