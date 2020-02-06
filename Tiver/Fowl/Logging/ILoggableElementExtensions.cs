namespace Tiver.Fowl.Logging
{
    using Logging;

    public static class ILoggableElementExtensions
    {
        /// <summary>
        /// Log action for an element
        /// </summary>
        /// <remarks>
        /// Nothing will be logged in case instance.Name is null or empty
        /// </remarks>
        /// <param name="instance">Initiator of action</param>
        /// <param name="action">Description of action</param>
        public static void LogAction(this ILoggableElement instance, string action)
        {
            if (string.IsNullOrEmpty(instance.Name))
            {
                return;
            }

            using (LogProvider.OpenMappedContext("LogType", "ElementAction"))
            {
                Logger.InfoFormat("{Name} :: {Action}", instance.Name, action);
            }
        }

        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
    }
}