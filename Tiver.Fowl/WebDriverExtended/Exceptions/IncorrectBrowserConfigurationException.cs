namespace Tiver.Fowl.WebDriverExtended.Exceptions
{
    using System;

    public class IncorrectBrowserConfigurationException : Exception
    {
        public IncorrectBrowserConfigurationException()
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
    }
}
