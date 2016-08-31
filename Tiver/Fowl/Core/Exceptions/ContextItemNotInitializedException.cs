namespace Tiver.Fowl.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ContextItemNotInitializedException : Exception, ISerializable
    {
        public ContextItemNotInitializedException()
        {
        }

        public ContextItemNotInitializedException(string message)
            : base(message)
        {
        }

        public ContextItemNotInitializedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ContextItemNotInitializedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
