namespace Tests.Logging
{
    using System.IO;
    using NUnit.Framework;
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
                .WriteTo.LiterateConsole()
                .WriteTo.File(new JsonFormatter(), Path.Combine(TestContext.CurrentContext.TestDirectory, "./log.txt"))
                .CreateLogger();
            configured = true;
        }

        private static bool configured;
    }
}