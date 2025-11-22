namespace Tiver.Fowl.Logging
{
    using System.IO;
    using Serilog;
    using Serilog.Extensions.Logging;
    using Serilog.Formatting.Json;
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
                .WriteTo.File(new JsonFormatter(), Path.Combine(Directory.GetCurrentDirectory(), "./log.txt"))
                .CreateLogger();
            configured = true;

            Wait.SetLogger(new SerilogLoggerProvider(Log.Logger, false).CreateLogger("Wait"));
        }

        private static bool configured;
    }
}
