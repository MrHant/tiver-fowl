namespace Tiver.Fowl.Core.Context
{
    public class NullContextItem
    {
        private object key;

        public NullContextItem()
        {
            this.key = null;
        }

        public NullContextItem(object key)
        {
            this.key = key;
        }

        public override string ToString()
        {
            var message = "Context item not found.";
            if (this.key != null)
            {
                message = $"Context item not found. for key '{this.key}'.";
            }

            return message;
        }
    }
}
