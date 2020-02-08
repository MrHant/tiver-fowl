namespace Tiver.Fowl.TestingBase
{
    using Serilog;
    using Serilog.Core;

    public static class IBaseTestExtensions
    {
        public static int GetNextStepNumber(this IBaseTest test)
        {
            test.Step = test.Step ?? 0;
            test.Step++;
            return test.Step.Value;
        }

        public static void LogStep(this IBaseTest test, string text)
        {
            var logStep = Log.ForContext("LogType", "TestStep");
            logStep.Information("Step #{Step} :: {Text}", test.GetNextStepNumber(), text);
        }
    }
}
