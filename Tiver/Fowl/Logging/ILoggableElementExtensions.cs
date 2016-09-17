namespace Tiver.Fowl.Logging
{
    using Serilog;

    public static class ILoggableElementExtensions
    {
        public static void LogAction(this ILoggableElement instance, string action)
        {
            Log.ForContext("LogType", "ElementAction").Information($"{{Name}} :: {{Action}}", instance.Name, action);
        }
    }
}