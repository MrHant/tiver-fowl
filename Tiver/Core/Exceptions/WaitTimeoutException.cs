namespace Tiver.Core.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class WaitTimeoutException : Exception, ISerializable
    {
        public WaitTimeoutException()
            : base()
        {
        }

        public WaitTimeoutException(string message)
            : base(message)
        {
        }

        public WaitTimeoutException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected WaitTimeoutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
