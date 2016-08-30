namespace Tiver.Fowl.Core.Context
{
    using System.Collections.Generic;

    public class Storage : IStorage
    {
        private Dictionary<object, object> items;

        public Storage()
        {
            this.items = new Dictionary<object, object>();
        }

        private Dictionary<object, object> Items
        {
            get
            {
                this.items = this.items ?? new Dictionary<object, object>();
                return this.items;
            }
        }

        public void Write(object key, object value)
        {
            this.Items.Add(key, value);
        }

        public object Read(object key)
        {
            object result = null;
            var success = this.Items.TryGetValue(key, out result);
            if (success)
            {
                return result;
            }
            else
            {
                return new NullContextItem(key);
            }
        }

        public void Clear()
        {
            this.Items.Clear();
        }
    }
}
