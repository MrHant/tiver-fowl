namespace Tiver.Fowl.TestingBase
{
    using Core.Context;
    using Serilog;

    public static class IBaseTestExtensions
    {
        public static int GetNextStepNumber(this IBaseTest test)
        {
            TestExecutionContext.TestStep++;
            return TestExecutionContext.TestStep;
        }

        public static void LogStep(this IBaseTest test, string text)
        {
            var logStep = Log.ForContext("LogType", "TestStep");
            logStep.Information("Step #{Step} :: {Text}", test.GetNextStepNumber(), text);
        }
    }
}
