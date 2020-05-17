namespace XXXXX.Logging
{
    using Serilog;
    using Serilog.Extensions.Logging;
    using Serilog.Formatting.Json;
    using Tiver.Fowl.Logging;
    using Tiver.Fowl.Waiting;

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
            
			//// Add package 'Serilog.Extensions.Logging' and uncomment line below to enable logging for Tiver.Fowl.Wait package
            // Wait.SetLogger(new SerilogLoggerProvider(Log.Logger, false).CreateLogger("Wait"));
        }

        private static bool configured;
    }
}
