namespace Tests.Logging
{
    using System;
    using System.IO;
    using NUnit.Framework;
    using Serilog;
    using Serilog.Extensions.Logging;
    using Serilog.Formatting.Json;
    using Tiver.Fowl.Core.Context;
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

            // Generate unique SessionId with milliseconds and random suffix to prevent collisions
            var timestamp = System.DateTime.Now;
            var randomSuffix = new Random().Next(1000, 9999);
            TestExecutionContext.SessionId = $"{timestamp:yyyyMMdd-HHmmss-fff}-{randomSuffix}";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.With(new TestNameEnricher())
                .Enrich.With(new SessionIdEnricher())
                .WriteTo.Console()
                .WriteTo.File(new JsonFormatter(), Path.Combine(TestContext.CurrentContext.TestDirectory, "./log.txt"))
                .CreateLogger();
            configured = true;

            Wait.SetLogger(new SerilogLoggerProvider(Log.Logger, false).CreateLogger("Wait"));

            Log.Information("Test session started");
        }

        private static bool configured;
    }
}