namespace Tiver.Fowl.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class IncorrectApplicationConfigurationException : Exception, ISerializable
    {
        public IncorrectApplicationConfigurationException()
        {
        }

        public IncorrectApplicationConfigurationException(string message)
            : base(message)
        {
        }

        public IncorrectApplicationConfigurationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected IncorrectApplicationConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
