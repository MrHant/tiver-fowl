namespace Tiver.Fowl.Logging
{
    using Serilog;
    using Serilog.Formatting.Json;

    public static class Logger
    {
        public static void Configure()
        {
            if (configured)
            {
                return;
            }

            Log.Logger = new LoggerConfiguration()
                .Enrich.With(new TestNameEnricher())
                .WriteTo.LiterateConsole()
                .WriteTo.File(new JsonFormatter(), "./log.txt")
                .CreateLogger();
            configured = true;
        }

        private static bool configured;
    }
}
