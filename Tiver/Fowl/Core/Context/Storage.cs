namespace Tiver.Fowl.Core.Context
{
    using System;
    using System.Collections.Generic;
    using Exceptions;

    public class Storage : IStorage
    {
        private Dictionary<string, object> items;

        public Storage()
        {
            items = new Dictionary<string, object>();
        }

        private Dictionary<string, object> Items
        {
            get
            {
                items = items ?? new Dictionary<string, object>();
                return items;
            }
        }

        public void Write(string key, object value)
        {
            if (Items.ContainsKey(key))
            {
                Items[key] = value;
            }
            else
            {
                Items.Add(key, value);
            }
        }

        public object Read(string key)
        {
            try
            {
                return Items[key];
            }
            catch(Exception ex)
            {
                throw new StorageKeyNotFoundException($"Storage item for key '{key}' not found", ex);
            }
        }

        public void Clear()
        {
            Items.Clear();
        }
    }
}
