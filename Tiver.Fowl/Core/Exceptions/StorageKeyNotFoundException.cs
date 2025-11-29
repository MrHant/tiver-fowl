namespace Tiver.Fowl.Core.Exceptions
{
    using System;

    public class StorageKeyNotFoundException : Exception
    {
        public StorageKeyNotFoundException()
        {
        }

        public StorageKeyNotFoundException(string message)
            : base(message)
        {
        }

        public StorageKeyNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}