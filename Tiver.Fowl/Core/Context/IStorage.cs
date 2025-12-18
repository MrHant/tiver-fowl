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
        /// Read an item from storage with typed return value
        /// </summary>
        /// <typeparam name="T">Type to cast the value to</typeparam>
        /// <exception cref="StorageKeyNotFoundException">Item not found in Storage for provided <paramref name="key"/></exception>
        /// <param name="key">Key for looked up item</param>
        /// <returns>Value of found item cast to T</returns>
        T Read<T>(string key);

        /// <summary>
        /// Read an item from storage
        /// In case not existing key - create new item with default value and return it
        /// </summary>
        /// <param name="key">Key for looked up item</param>
        /// <param name="defaultValue">Value for new item to be added in case missing</param>
        /// <returns>Value of found (or created) item</returns>
        object ReadOrInit(string key, object defaultValue);

        /// <summary>
        /// Read an item from storage with typed return value
        /// In case not existing key - create new item with default value and return it
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="key">Key for looked up item</param>
        /// <param name="defaultValue">Value for new item to be added in case missing</param>
        /// <returns>Value of found (or created) item cast to T</returns>
        T ReadOrInit<T>(string key, T defaultValue);

        void Clear();
    }
}
