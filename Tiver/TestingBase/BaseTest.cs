namespace Tiver.TestingBase
{
    public abstract class BaseTest
    {
        private TestExecutionContext context = null;

        internal BaseTest()
        {
        }

        public TestExecutionContext Context
        {
            get
            {
                return this.context = this.context ?? new TestExecutionContext(GetType());
            }
        }
    }
}
