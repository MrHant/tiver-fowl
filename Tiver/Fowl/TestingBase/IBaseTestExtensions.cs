namespace Tiver.Fowl.TestingBase
{
    using Logging;

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
            using (LogProvider.OpenMappedContext("LogType", "TestStep"))
            {
                Logger.InfoFormat("Step #{Step} :: {Text}", test.GetNextStepNumber(), text);
            }
        }

        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
    }
}
