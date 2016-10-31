namespace Tiver.Fowl.TestingBase
{
    using Serilog;

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
            Log.ForContext("LogType", "TestStep").Information($"Step #{{Step}} :: {{Text}}", test.GetNextStepNumber(), text);
        }
    }
}
