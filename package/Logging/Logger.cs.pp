namespace $rootnamespace$.Logging
{
    using Serilog;
    using Serilog.Formatting.Json;
    using Tiver.Fowl.Logging;

    public class Logger
    {
        public static void Configure()
        {
            if (configured)
            {
                return;
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.With(new TestNameEnricher())
                .WriteTo.Console()
                .WriteTo.File(new JsonFormatter(), "./log.txt")
                .CreateLogger();
            configured = true;
        }

        private static bool configured;
    }
}
