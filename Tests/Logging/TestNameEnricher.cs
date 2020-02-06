namespace Tests.Logging
{
    using Serilog.Core;
    using Serilog.Events;
    using Tiver.Fowl.Core.Context;

    public class TestNameEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "TestName", TestExecutionContext.TestName));
        }
    }
}