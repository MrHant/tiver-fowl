namespace Tiver.Fowl.TestingBase
{
    using Contracts;
    using Core.Context;

    public abstract class BaseTest : IBaseTest
    {
        internal BaseTest()
        {
            TestExecutionContext.TestType = GetType();
        }

        public abstract void Setup();

        public abstract void Teardown();
    }
}
