namespace Tiver.Fowl.TestingBase
{
    using Contracts;
    using Core.Context;
    using Serilog;

    public abstract class BaseTest : IBaseTest
    {
        internal BaseTest()
        {
            TestExecutionContext.TestType = GetType();
            step = 1;
        }

        public abstract void Setup();

        public abstract void Teardown();

        public void LogStep(string text)
        {
            Log.ForContext("LogType", "TestStep").Information($"Step #{{Step}} :: {{Text}}", step++, text);
        }

        private static int step;
    }
}
