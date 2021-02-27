namespace Tiver.Fowl.Core.Context
{
    using Exceptions;

    public interface IStorage
    {
        void Write(string key, object value);

        /// <summary>
        /// Read an item from storage
        /// </summary>
        /// <exception cref="StorageKeyNotFoundException">Item not found in Storage for provided <paramref name="key"/></exception>
        /// <param name="key">Key for looked up item</param>
        /// <returns>Value of found item</returns>
        object Read(string key);

        /// <summary>
        /// Read an item from storage
        /// In case not existing key - create new item with default value and return it
        /// </summary>
        /// <param name="key">Key for looked up item</param>
        /// <param name="defaultValue">Value for new item to be added in case missing</param>
        /// <returns>Value of found (or created) item</returns>
        object ReadOrAdd(string key, object defaultValue);

        void Clear();
    }
}
