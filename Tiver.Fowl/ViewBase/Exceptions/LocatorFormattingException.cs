namespace Tiver.Fowl.ViewBase.Exceptions
{
    using System;

    public class LocatorFormattingException : Exception
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
    }
}