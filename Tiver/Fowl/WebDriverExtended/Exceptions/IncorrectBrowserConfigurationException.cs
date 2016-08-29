namespace Tiver.Fowl.WebDriverExtended.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class IncorrectBrowserConfigurationException : Exception, ISerializable
    {
        public IncorrectBrowserConfigurationException()
            : base()
        {
        }

        public IncorrectBrowserConfigurationException(string message)
            : base(message)
        {
        }

        public IncorrectBrowserConfigurationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected IncorrectBrowserConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
