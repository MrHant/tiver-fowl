namespace Tiver.Fowl.Logging
{
    using Serilog;
    using Serilog.Formatting.Json;
    using Serilog.Extensions.Logging;

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

            Tiver.Fowl.Waiting.Wait.SetLogger(new SerilogLoggerProvider(Log.Logger, false).CreateLogger("Wait"));
        }

        private static bool configured;
    }
}
