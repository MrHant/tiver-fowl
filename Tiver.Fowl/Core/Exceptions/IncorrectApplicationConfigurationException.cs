namespace Tiver.Fowl.Core.Exceptions
{
    using System;

    public class IncorrectApplicationConfigurationException : Exception
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
    }
}
