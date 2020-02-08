namespace Tiver.Fowl.Logging
{
    using Serilog.Core;
    using Serilog.Events;
    using Core.Context;

    public class TestNameEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "TestName", TestExecutionContext.TestName));
        }
    }
}