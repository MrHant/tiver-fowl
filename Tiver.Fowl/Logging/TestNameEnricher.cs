namespace Tiver.Fowl.Logging
{
    using Serilog.Core;
    using Serilog.Events;
    using Core.Context;

    public class TestNameEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            // Events are also emitted outside a test scope — session setup, report generation,
            // teardown. Serilog routes an enricher exception to SelfLog and drops that event's
            // enrichment, so resolving the name has to stay non-throwing.
            var testName = TestExecutionContext.CurrentTestNameOrNull;
            if (!string.IsNullOrEmpty(testName))
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TestName", testName));
            }
        }
    }
}