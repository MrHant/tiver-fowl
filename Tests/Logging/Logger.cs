namespace Tests.Logging
{
    using System.IO;
    using NUnit.Framework;
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
                .WriteTo.File(new JsonFormatter(), Path.Combine(TestContext.CurrentContext.TestDirectory, "./log.txt"))
                .CreateLogger();
            configured = true;
            
            Wait.SetLogger(new SerilogLoggerProvider(Log.Logger, false).CreateLogger("Wait"));
        }

        private static bool configured;
    }
}