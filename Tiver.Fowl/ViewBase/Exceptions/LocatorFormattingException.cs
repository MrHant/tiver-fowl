namespace Tiver.Fowl.ViewBase.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class LocatorFormattingException : Exception, ISerializable
    {
        public LocatorFormattingException()
        {
        }

        public LocatorFormattingException(string message)
            : base(message)
        {
        }

        public LocatorFormattingException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected LocatorFormattingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}