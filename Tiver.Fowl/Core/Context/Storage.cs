namespace Tiver.Fowl.Core.Context
{
    using System.Collections.Concurrent;
    using Exceptions;

    public class Storage : IStorage
    {
        private readonly ConcurrentDictionary<string, object> _items = new();

        public void Write(string key, object value)
        {
            _items[key] = value;
        }

        public object Read(string key)
        {
            if (_items.TryGetValue(key, out var value))
            {
                return value;
            }

            throw new StorageKeyNotFoundException($"Storage item for key '{key}' not found");
        }

        public T Read<T>(string key)
        {
            return (T)Read(key);
        }

        public object ReadOrInit(string key, object defaultValue)
        {
            return _items.GetOrAdd(key, defaultValue);
        }

        public T ReadOrInit<T>(string key, T defaultValue)
        {
            return (T)_items.GetOrAdd(key, defaultValue);
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}
