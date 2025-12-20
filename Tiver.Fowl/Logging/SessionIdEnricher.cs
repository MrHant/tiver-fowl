namespace Tiver.Fowl.Logging
{
    using Serilog.Core;
    using Serilog.Events;
    using Core.Context;

    /// <summary>
    /// Enriches Serilog events with the current test session identifier.
    /// SessionId format: yyyyMMdd-HHmmss-fff-RRRR where RRRR is a random 4-digit suffix
    /// to prevent collisions in parallel or rapid test executions.
    /// </summary>
    public class SessionIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var sessionId = TestExecutionContext.SessionId;
            if (!string.IsNullOrEmpty(sessionId))
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SessionId", sessionId));
            }
        }
    }
}
