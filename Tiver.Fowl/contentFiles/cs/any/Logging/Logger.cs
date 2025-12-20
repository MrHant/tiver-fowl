namespace Tiver.Fowl.Logging
{
    using System;
    using Serilog;
    using Serilog.Formatting.Json;
    using Serilog.Extensions.Logging;
    using Tiver.Fowl.Core.Context;

    public class Logger
    {
        public static void Configure()
        {
            if (configured)
            {
                return;
            }

            // Generate unique SessionId with milliseconds and random suffix to prevent collisions
            var timestamp = DateTime.Now;
            var randomSuffix = new Random().Next(1000, 9999);
            TestExecutionContext.SessionId = $"{timestamp:yyyyMMdd-HHmmss-fff}-{randomSuffix}";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.With(new TestNameEnricher())
                .Enrich.With(new SessionIdEnricher())
                .WriteTo.Console()
                .WriteTo.File(new JsonFormatter(), "./log.txt")
                .CreateLogger();
            configured = true;

            Tiver.Fowl.Waiting.Wait.SetLogger(new SerilogLoggerProvider(Log.Logger, false).CreateLogger("Wait"));
        }

        private static bool configured;
    }
}
